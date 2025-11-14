using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{
    public abstract class AItemRuntimeDataBase : IItemRuntimeData
    {
        private int _stackCount;
        private readonly ItemData _itemData;

        public ItemData BaseData => _itemData;
        public int StackCount => _stackCount;
        public int MaxStack => BaseData.MaxStack;
        public ItemType ItemType => BaseData.ItemType;
        public bool CanStack => MaxStack > 1;

        protected AItemRuntimeDataBase(ItemData data, int initialCount = 1)
        {
            _itemData = data;
            _stackCount = Mathf.Clamp(initialCount, 0, MaxStack);
        }

        //どれだけ加算できるか判断する
        public virtual int AddToStack(int amount)
        {
            if (!CanStack || amount <= 0)
                return amount;

            int space = MaxStack - _stackCount;
            int toAdd = Mathf.Min(space, amount);
            _stackCount += toAdd;
            return amount - toAdd;
        }

        //純粋に数量を減らす（MergeやSplit用）
        public virtual int ReduceStack(int amount)
        {
            if (amount <= 0) return 0;
            int reduced = Mathf.Min(_stackCount, amount);
            _stackCount -= reduced;
            return reduced;
        }

        //こいつはもう一個を削除する機能を持っていない
        //スタック同士を統合し、統合結果を返す
        //trueなら結合できた（otherが空になり、削除可能）　falseなら余っている(otherは残す必要あり）
        public virtual bool Merge(IItemRuntimeData other)
        {
            if (other == null || other.BaseData != BaseData)
                return false;

            if (!CanStack)
                return false;

            int remainder = AddToStack(other.StackCount); //溢れチェックをAddToStackに任せる
            int consumed = other.StackCount - remainder;  //実際に吸収できた数

            other.ReduceStack(consumed);

            // remainderが0なら完全統合。残っていれば溢れパターン
            return remainder <= 0;
        }

        //Split（=新しいインスタンスを生成）するのはインベントリの役目、ここには不要
    }
}
