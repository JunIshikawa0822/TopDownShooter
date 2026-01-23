using System;
using System.Collections.Generic;
using UnityEngine;

//「どの部位に」「どんなタイプのアタッチメントを装着できるか」を定義するテンプレート。
[Serializable]
public class GunAttachmentSlotData
{
    [SerializeField] private string _slotID;
    [SerializeField] private AttachmentType _slotType;
    [SerializeField] private string[] _allowedTags;
    [SerializeField] private GunAttachmentData _defaultAttachmentData;//null許容

    [Header("スロット座標")]
    [SerializeField] private Vector3 _slotPosition;
    [SerializeField] private Quaternion _slotRotation;

    public string SlotID => _slotID;
    public AttachmentType SlotType => _slotType;
    public IReadOnlyList<string> AllowedTags => _allowedTags;//違う方法で許可を表せる（optional）
    public GunAttachmentData DefaultAttachmentData => _defaultAttachmentData;
    public Vector3 SlotPosition => _slotPosition;
    public Quaternion SlotRotation => _slotRotation;
}
