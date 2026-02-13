using System.Collections.Generic;
using UnityEngine;
using System;

//制作意図:キャラクターの全ステータスを管理し、5層構造の計算ロジックを実行する。
//用途: ゲーム内のダメージ計算、移動、回復処理など、全てこのクラスを参照する。
public class Stats
{
    // ----------------------------------------------------------------
    // 5つのステータス階層
    // ----------------------------------------------------------------
    private StatsData _baseStats;
    private StatsData _equipStats;
    private StatsData _buffStats;
    private StatsData _debuffStats;
    private StatsData _finalStats;

    //----------------------------------------------------------------
    //現在値の管理 (エンティティの状態)
    //----------------------------------------------------------------
    private float _currentHP;
    private float _currentStamina;

    #region プロパティ
    public StatsData BaseStats => _baseStats;//素ステータス (レベルアップで上昇)
    public StatsData EquipmentStats => _equipStats;//装備補正
    public StatsData BuffStats => _buffStats;//バフによる一時的な増加
    public StatsData DebuffStats => _debuffStats;//デバフによる一時的な減少
    public StatsData FinalStats =>  _finalStats;//実際に使う合算結果

    public float CurrentHP => _currentHP;
    public float MaxHP => FinalStats.MaxHP;
    public float CurrentStamina => _currentStamina;
    public float MaxStamina => FinalStats.MaxStamina;
    
    //現在の総合的な回復/減少率をFinalStatsから参照
    public float TotalHPRegenValue => FinalStats.HPRegenValue - FinalStats.BleedValue;
    public float TotalStaminaRegenValue => FinalStats.StaminaRegenValue;

    #endregion

    //----------------------------------------------------------------
    //コンストラクタ
    //----------------------------------------------------------------
    //使い方: Stats stats = new Stats(100f, 50f); => 基本HP100, 基本スタミナ50
    public Stats(float baseHP, float baseStamina)
    {
        // レイヤーの初期化とベース値の設定
        _baseStats = new StatsData { MaxHP = baseHP, MaxStamina = baseStamina };
        _equipStats = new StatsData();
        _buffStats = new StatsData();
        _debuffStats = new StatsData();
        _finalStats = new StatsData();
        
        // 初回計算と現在値の初期化
        RecalculateFinalStats();
        _currentHP = MaxHP;
        _currentStamina = MaxStamina;
    }

    //----------------------------------------------------------------
    //最終ステータスの計算ロジック
    //----------------------------------------------------------------
    //5つのレイヤーを合算し、FinalStatsを更新する。
    //装備変更、バフ/デバフの適用/解除など、最大値に関わる変動があった際に必ず呼び出され、データの整合性を保証する。

    public void RecalculateFinalStats()
    {
        // 現在の最大値を記録 (現在値の調整に使用)
        float oldMaxHP = MaxHP;
        float oldMaxStamina = MaxStamina;
        
        // FinalStatsをリセット
        FinalStats.Reset();
        
        // 1. 全てのレイヤーを単純に合算
        FinalStats.Add(BaseStats);
        FinalStats.Add(EquipmentStats);
        FinalStats.Add(BuffStats);
        FinalStats.Add(DebuffStats);
        
        // 2. 現在値の調整 (新しい最大値に基づいて現在値を調整)
        
        float newMaxHP = FinalStats.MaxHP;
        if (newMaxHP > oldMaxHP)
        {
            // 最大値が増加した場合、現在HPもその差分だけ回復 (例: 装備をつけた時)
            _currentHP += (newMaxHP - oldMaxHP);
        }
        // 現在のHPが新しい最大値を超えないように制限 (例: 装備を外した時)
        _currentHP = Math.Clamp(_currentHP, 0f, newMaxHP);

        // スタミナも同様に調整
        float newMaxStamina = FinalStats.MaxStamina;
        if (newMaxStamina > oldMaxStamina)
        {
            _currentStamina += (newMaxStamina - oldMaxStamina);
        }
        _currentStamina = Math.Clamp(_currentStamina, 0f, newMaxStamina);
    }
    
    //----------------------------------------------------------------
    //階層への変更と更新の仕組み
    //----------------------------------------------------------------

    //EquipmentStatsをリセットし、新しい装備の合計値で再構築する。
    public void UpdateEquipment(List<StatsData> equipmentEffects)
    {
        EquipmentStats.Reset(); 
        foreach (StatsData effect in equipmentEffects)
        {
            EquipmentStats.Add(effect);
        }
        RecalculateFinalStats(); 
    }

    //EffectManagerからバフ/デバフの適用または解除の指示を受け取る。
    public void ApplyEffectToLayer(StatsData effectData, bool isBuff, bool apply)
    {
        StatsData targetLayer = isBuff ? BuffStats : DebuffStats;
        
        //解除時（apply=false）は、効果をマイナスにしてAddすることで減算を実現
        if(!apply)
        {
            // 減算用のStatsDataを作成（または元の値を反転）
            effectData.HPRegenValue *= -1; 
            effectData.BleedValue *= -1; 
            effectData.StaminaRegenValue *= -1;
            effectData.MaxHP *= -1;
            effectData.MaxStamina *= -1;
        }

        targetLayer.Add(effectData); // 該当レイヤーに加算/減算
        RecalculateFinalStats(); // Regen値やMax値が変わるため再計算
    }

    //----------------------------------------------------------------
    //時間経過による現在値の更新
    //----------------------------------------------------------------

    //時間経過による回復/減少を処理する (ゲームのUpdateループから呼び出す)
    //FinalStatsから確定したRegen/Bleed値に基づいて現在値を変動させる。
    //使い方:stats.UpdateCurrentValues(Time.deltaTime, player.IsMoving);
    public void UpdateCurrentValues(float deltaTime, bool isMoving)
    {
        //HPの更新 (リジェネ - 出血)
        //TotalHealthRegenはFinalStatsの計算結果を参照しているため、常に最新の値
        float hpChange = TotalHPRegenValue * deltaTime;
        _currentHP = Math.Clamp(_currentHP + hpChange, 0, MaxHP);

        // 2. スタミナの更新 (立ち止まっている時のみ回復)
        if (!isMoving)
        {
            float staminaChange = TotalStaminaRegenValue * deltaTime;
            _currentStamina = Math.Clamp(_currentStamina + staminaChange, 0, MaxStamina);
        }
        // 動いている時はStaminaRegenが0として扱われる（消費する場合は別途ロジックが必要）
    }
    
    //----------------------------------------------------------------
    //その他の現在値操作
    //----------------------------------------------------------------
    public void ApplyDamage(float amount)
    {
        _currentHP = Math.Clamp(_currentHP - amount, 0, MaxHP);
    }
}