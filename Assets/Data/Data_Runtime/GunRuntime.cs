using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Game.Data;

public class GunRuntime : AWeaponRuntimeBase
{
    //キーをあらかじめ登録
    public const string KEY_VELOCITY = "Velocity";
    public const string KEY_HORIZONTAL_RECOIL = "HorizontalRecoil";
    public const string KEY_VERTICAL_RECOIL = "VerticalRecoil";
    public const string KEY_SHOT_SPREAD = "ShotSpread";
    public const string KEY_BASE_SCATTER = "BaseScatter";
    public const string KEY_SPREAD_INCRIMENT = "ScatterIncriment";
    public const string KEY_MAX_SPREAD = "MaxScatter";
    public const string KEY_RELOAD_TIME = "ReloadTime";
    public const string KEY_ERGONOMICS = "Ergonomics";
    public const string KEY_MAX_RANGE = "MaxRange";

    private AttachmentRuntime_Magazine _currentMagazine = null;

    private uint _internalAmmoRemaining = 0;
    private AmmoData _loadedInternalAmmoData;
    private float _lastShotTime = -999f;
    public bool IsIntervalShooting => Time.time - _lastShotTime < FireInterval;

    public GunData GunData => ItemData as GunData;
    public AmmoData LoadAmmoData => GunData.IsInternalMagazine ? _loadedInternalAmmoData : _currentMagazine.LoadedAmmoData;

    public FireType FireType => GunData.FireType;
    public int BurstCount => GunData.BurstCount;
    public int SimulNum => GunData.SimulNum;
    public float FireInterval => GunData.FireInterval;
    public float Velocity => _baseStats[KEY_VELOCITY] + GetEquipOffsetStat(KEY_VELOCITY);
    public float HorizontalRecoil => _baseStats[KEY_HORIZONTAL_RECOIL] + GetEquipOffsetStat(KEY_HORIZONTAL_RECOIL);
    public float VerticalRecoil => _baseStats[KEY_VERTICAL_RECOIL] + GetEquipOffsetStat(KEY_VERTICAL_RECOIL);
    public float ShotSpread => _baseStats[KEY_SHOT_SPREAD] + GetEquipOffsetStat(KEY_SHOT_SPREAD);
    public float BaseScatter => _baseStats[KEY_BASE_SCATTER] + GetEquipOffsetStat(KEY_BASE_SCATTER);
    public float ScatterIncriment => _baseStats[KEY_SPREAD_INCRIMENT] + GetEquipOffsetStat(KEY_SPREAD_INCRIMENT);
    public float MaxScatter => _baseStats[KEY_MAX_SPREAD] + GetEquipOffsetStat(KEY_MAX_SPREAD);
    private float _internalCurrentScatter;//拡散量。これがBaseScatterからMaxScatterの範囲で変動するイメージ
    public float ReloadTime => _baseStats[KEY_RELOAD_TIME] + GetEquipOffsetStat(KEY_RELOAD_TIME);
    public float Ergonomics => _baseStats[KEY_ERGONOMICS] + GetEquipOffsetStat(KEY_ERGONOMICS);
    public float MaxRange => _baseStats[KEY_MAX_RANGE] + GetEquipOffsetStat(KEY_MAX_RANGE);

    public float CurrentScatter
    {
        get
        {
            float timePassed = Time.time - _lastShotTime;
            // TODO: 拡散の回復速度をとりあえず増加の1/3にしている
            float recovery = (ScatterIncriment / 3f) * timePassed;
            _internalCurrentScatter = Mathf.Max(BaseScatter, _internalCurrentScatter - recovery);
            return _internalCurrentScatter;
        }
    }

    public GunRuntime(GunData gunData, int initialCount = 1, Guid? runtimeGuid = null) : base(gunData, initialCount, runtimeGuid)
    {
        _baseStats = new()
        {
            { KEY_VELOCITY, gunData.Velocity },
            { KEY_HORIZONTAL_RECOIL, gunData.HorizontalRecoil },
            { KEY_VERTICAL_RECOIL, gunData.VerticalRecoil },
            { KEY_SHOT_SPREAD, gunData.ShotSpread },
            { KEY_BASE_SCATTER, gunData.BaseScatter },
            { KEY_SPREAD_INCRIMENT, gunData.ScatterIncriment },
            { KEY_MAX_SPREAD, gunData.MaxScatter },
            { KEY_RELOAD_TIME, gunData.ReloadTime },
            { KEY_ERGONOMICS, gunData.Ergonomics },
            { KEY_MAX_RANGE, gunData.MaxRange }
        };

        _internalAmmoRemaining = (uint)GunData.InternalCapacity;
        _internalCurrentScatter = BaseScatter;
    }
    //アタッチメント付け替えがすでに手動リロード処理になっているので、ここでは書かない
    //代わりに、自動リロードにあたる処理追加
    public virtual bool TryEquipAndSwapMagazine(AttachmentRuntime_Magazine magazine, out AttachmentRuntime swapAttachment)
    {
        AttachmentSlot magazineSlot = FindSlot(AttachmentType.Magazine).FirstOrDefault();
        bool isSucceed = TryEquipAndSwapAttachment(magazineSlot, magazine, out swapAttachment);

        return isSucceed;
    }

    //弾丸を直接内部マガジンに入れ込む
    public virtual bool TryInternalReload(AmmoRuntime ammoRuntime)
    {
        if (!GunData.IsInternalMagazine) return false;
        int required = (int)GunData.MaxStack - (int)_internalAmmoRemaining;
        if (required <= 0) return false;

        int loadCount = Mathf.Min(ammoRuntime.Stack, required);
        _internalAmmoRemaining += (uint)loadCount;
        ammoRuntime.ReduceStack(loadCount);
        _loadedInternalAmmoData = ammoRuntime.AmmoData;

        return true;
    }

    //現在使用中のマガジンを設定する
    public override bool TryEquipAndSwapAttachment(AttachmentSlot slot, AttachmentRuntime attachment, out AttachmentRuntime swapAttachment)
    {
        bool isSucceed = base.TryEquipAndSwapAttachment(slot, attachment, out swapAttachment);
        if (isSucceed && (attachment is AttachmentRuntime_Magazine magazine))
        {
            _currentMagazine = magazine;
        }

        return isSucceed;
    }

    //TODO: 無限弾の仕様考えておく
    public virtual bool CanConsume(bool isConsume = true)
    {
        if (!GunData.IsInternalMagazine)
        {
            if (_currentMagazine == null) return false;
            return _currentMagazine.TryConsume(isConsume);
        }
        else
        {
            if (_internalAmmoRemaining <= 0) return false;

            _internalAmmoRemaining--;

            if (_internalAmmoRemaining <= 0)
            {
                _loadedInternalAmmoData = null;
            }

            return true;
        }
    }

    public void RecordShotTime()
    {
        _lastShotTime = Time.time;
    }

    //撃つたびに反動で精度が落ちる処理
    public void IncrimentScatter()
    {
        _internalCurrentScatter = Math.Clamp(_internalCurrentScatter + ScatterIncriment, BaseScatter, MaxScatter);
    }
}
