using System;
// ==========================================================
// EquipmentSlot：装備箇所
// ==========================================================
[Flags]
public enum EquipmentSlot
{
    None = 0,
    MainHand = 1 << 0,
    OffHand = 1 << 1,
    BothHands = 1 << 2,
    Back = 1 << 3,
    Waist = 1 << 4,
}
