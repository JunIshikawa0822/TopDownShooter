using System;
using System.Collections.Generic;
using UnityEngine;

//「どの部位に」「どんなタイプのアタッチメントを装着できるか」を定義するテンプレート。
[Serializable]
public class AttachmentSlotData
{
    [SerializeField] private string _slotID;
    [SerializeField] private AttachmentType _slotType;
    [SerializeField] private List<string> _allowedTags;
    [SerializeField] private AttachmentData _defaultAttachmentData;//null許容

    [Header("スロット座標")]
    [SerializeField] private Vector3 _slotPosition;
    [SerializeField] private Quaternion _slotRotation;

    public string SlotID => _slotID;
    public AttachmentType SlotType => _slotType;
    public List<string> AllowedTags => _allowedTags;//違う方法で許可を表せる（optional）
    public AttachmentData DefaultAttachmentData => _defaultAttachmentData;
    public Vector3 SlotPosition => _slotPosition;
    public Quaternion SlotRotation => _slotRotation;
}
