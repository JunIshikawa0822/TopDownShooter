using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Game.Data;

public class GunRuntime : AWeaponRuntimeBase
{
    public GunData GunData =>  ItemData as GunData;
    private AttachmentRuntime_Magazine _currentMagazine = null;
    public GunRuntime(GunData gunData, int initialCount = 1) : base(gunData, initialCount)
    {
        _baseStats = new()
        {
            { "Velocity", gunData.Velocity },
            { "HorizontalRecoil", gunData.HorizontalRecoil },
            { "VerticalRecoil", gunData.VerticalRecoil },
            { "BaseSpread", gunData.BaseSpread },
            { "SpreadIncriment", gunData.SpreadIncriment },
            { "MaxSpread", gunData.MaxSpread },
            { "ReloadTime", gunData.ReloadTime },
            { "Ergonimics", gunData.Ergonomics },
            { "MaxRange", gunData.MaxRange }
        };
    }
    //アタッチメント付け替えがすでに手動リロード処理になっているので、ここでは書かない
    //代わりに、自動リロード処理追加
    public virtual bool TryEquipAndSwapMagazine(AttachmentRuntime_Magazine magazine, out AttachmentRuntime swapAttachment)
    {
        AttachmentSlot magazineSlot = FindSlot(AttachmentType.Magazine).FirstOrDefault();
        bool isSucceed = TryEquipAndSwapAttachment(magazineSlot, magazine, out swapAttachment);

        return isSucceed;
    }

    //現在使用中のマガジンに設定する
    public override bool TryEquipAndSwapAttachment(AttachmentSlot slot, AttachmentRuntime attachment, out AttachmentRuntime swapAttachment)
    {
        bool isSucceed = base.TryEquipAndSwapAttachment(slot, attachment, out swapAttachment);
        if(isSucceed && (attachment is AttachmentRuntime_Magazine magazine))
        {
            _currentMagazine = magazine;
        }

        return true;
    }

    //TODO: マガジンの弾丸消費をはじめとした、銃特有の挙動

    protected override AItemRuntimeBase CreateCopy(int initialCount)
    {
        return new GunRuntime(GunData, initialCount);
    }
}
