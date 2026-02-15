using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Game.Data;

// 3) 非ジェネリック共通抽象（Transform / Visual / Pool 対応）
public abstract class AWeaponBase : APooledObject
{
    [SerializeField] protected Transform _visualTrans;
    protected GameObject _visualInstance;
    // protected AWeaponRuntimeBase _weaponRuntime;
    protected readonly Dictionary<WeaponAnchorType, Transform> _anchorCache = new();

    public virtual void Initialize(AWeaponRuntimeBase runtime)
    {
        //_weaponRuntime = runtime;
    }

    public virtual void VisualSet(GameObject visualInstance)
    {
        //既存の見た目オブジェクトを破棄
        if (_visualInstance != null)
        {
            Addressables.ReleaseInstance(_visualInstance);
            _anchorCache.Clear();
            _visualInstance = null;
        }

        //親子関係を設定、参照を保持
        //falseでローカル座標を維持
        visualInstance.transform.SetParent(_visualTrans, false);
        _visualInstance = visualInstance;

        // 全てのアンカーを一度に取得してキャッシュ
        WeaponAnchor[] anchors = visualInstance.GetComponentsInChildren<WeaponAnchor>();
        foreach (WeaponAnchor anchor in anchors)
        {
            if (!_anchorCache.ContainsKey(anchor.AnchorType))
            {
                _anchorCache.Add(anchor.AnchorType, anchor.transform);
            }
        }
    }

    public abstract void AttackStart(bool isAttackSupportInput);
    public abstract void AttackProcess(bool isAttackSupportInput);
    public abstract void AttackEnd(bool isAttackSupportInput);

    public override void ReturnToPool()
    {
        Addressables.ReleaseInstance(_visualInstance);
        base.ReturnToPool();
    }

    protected Transform GetAnchor(WeaponAnchorType type)
    {
        _anchorCache.TryGetValue(type, out Transform target);
        return target;
    }
}