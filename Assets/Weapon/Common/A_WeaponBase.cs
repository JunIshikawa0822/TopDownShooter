using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Game.Data;

// 3) 非ジェネリック共通抽象（Transform / Visual / Pool 対応）
public abstract class AWeaponBase : APooledObject
{
    [SerializeField] protected Transform _visualTrans;
    [SerializeField] protected GameObject _visual;

    public abstract void Initialize(AWeaponRuntimeDataBase data);

    public virtual void VisualSet(GameObject visualInstance)
    {
        //既存の見た目オブジェクトを破棄
        if (_visual != null)
        {
            Addressables.ReleaseInstance(_visual);
            _visual = null;
        }

        //親子関係を設定、参照を保持
        //falseでローカル座標を維持
        visualInstance.transform.SetParent(_visualTrans, false);
        _visual = visualInstance;
    }

    public abstract void AttackStart();
    public abstract void AttackProcess();
    public abstract void AttackEnd();

    public override void ReturnToPool()
    {
        Addressables.ReleaseInstance(_visual);
        base.ReturnToPool();
    }
}