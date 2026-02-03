using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Game.Data;

public class GunRuntime : AWeaponRuntimeBase
{
    //キーをあらかじめ登録
    public const string KEY_VELOCITY = "Velocity";
    public const string KEY_HORIZONTAL_RECOIL = "HorizontalRecoil";
    public const string KEY_VERTICAL_RECOIL = "VerticalRecoil";
    public const string KEY_BASE_SPREAD = "BaseSpread";
    public const string KEY_SPREAD_INCRIMENT = "SpreadIncriment";
    public const string KEY_MAX_SPREAD = "MaxSpread";
    public const string KEY_RELOAD_TIME = "ReloadTime";
    public const string KEY_ERGONOMICS = "Ergonomics";
    public const string KEY_MAX_RANGE = "MaxRange";

    private AttachmentRuntime_Magazine _currentMagazine = null;

    private uint _internalAmmoRemaining = 0;
    private AmmoData _loadedInternalAmmoData;

    public GunData GunData => ItemData as GunData;
    public AmmoData LoadAmmoData => GunData.IsInternalMagazine ? _loadedInternalAmmoData : _currentMagazine.LoadedAmmoData;

    public FireType FireType => GunData.FireType;
    public int BurstCount => GunData.BurstCount;
    public float FireInterval => GunData.FireInterval;
    public float Velocity => _baseStats[KEY_VELOCITY] + GetEquipOffsetStat(KEY_VELOCITY);
    public float HorizontalRecoil => _baseStats[KEY_HORIZONTAL_RECOIL] + GetEquipOffsetStat(KEY_HORIZONTAL_RECOIL);
    public float VerticalRecoil => _baseStats[KEY_VERTICAL_RECOIL] + GetEquipOffsetStat(KEY_VERTICAL_RECOIL);
    public float BaseSpread => _baseStats[KEY_BASE_SPREAD] + GetEquipOffsetStat(KEY_BASE_SPREAD);
    public float SpreadIncriment => _baseStats[KEY_SPREAD_INCRIMENT] + GetEquipOffsetStat(KEY_SPREAD_INCRIMENT);
    public float MaxSpread => _baseStats[KEY_MAX_SPREAD] + GetEquipOffsetStat(KEY_MAX_SPREAD);
    public float ReloadTime => _baseStats[KEY_RELOAD_TIME] + GetEquipOffsetStat(KEY_RELOAD_TIME);
    public float Ergonomics => _baseStats[KEY_ERGONOMICS] + GetEquipOffsetStat(KEY_ERGONOMICS);
    public float MaxRange => _baseStats[KEY_MAX_RANGE] + GetEquipOffsetStat(KEY_MAX_RANGE);

    public GunRuntime(GunData gunData, int initialCount = 1) : base(gunData, initialCount)
    {
        _baseStats = new()
        {
            { KEY_VELOCITY, gunData.Velocity },
            { KEY_HORIZONTAL_RECOIL, gunData.HorizontalRecoil },
            { KEY_VERTICAL_RECOIL, gunData.VerticalRecoil },
            { KEY_BASE_SPREAD, gunData.BaseSpread },
            { KEY_SPREAD_INCRIMENT, gunData.SpreadIncriment },
            { KEY_MAX_SPREAD, gunData.MaxSpread },
            { KEY_RELOAD_TIME, gunData.ReloadTime },
            { KEY_ERGONOMICS, gunData.Ergonomics },
            { KEY_MAX_RANGE, gunData.MaxRange }
        };

        _internalAmmoRemaining = (uint)GunData.InternalCapacity;
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


}
