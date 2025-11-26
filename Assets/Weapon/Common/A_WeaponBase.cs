using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 3) 非ジェネリック共通抽象（Transform / Visual / Pool 対応）
public abstract class AWeaponBase : APooledObject
{
    [SerializeField] protected Transform _visualTrans;
    protected GameObject _visualPrefab;

    public virtual void VisualSet(GameObject newVisualPrefab)
    {
        //既存の見た目オブジェクトを破棄
        if (_visualPrefab != null)
        {
            GameObject.Destroy(_visualPrefab);
            _visualPrefab = null;
        }

        //親子関係を設定、参照を保持
        //falseでローカル座標を維持
        newVisualPrefab.transform.SetParent(_visualTrans, false);
        _visualPrefab = newVisualPrefab;
    }

    public abstract void AttackStart();
    public abstract void AttackProcess();
    public abstract void AttackEnd();
}