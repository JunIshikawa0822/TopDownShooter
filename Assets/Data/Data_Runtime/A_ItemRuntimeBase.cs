using System;
namespace Game.Data
{
    public abstract class AItemRuntimeBase : IItemRuntime
    {
        private readonly Guid _runtimeID;
        private readonly ItemData _data;
        private int _stackCount;

        public Guid RuntimeID => _runtimeID;
        public ItemData ItemData => _data;
        public ItemVisualData VisualData => _data.VisualData;
        public ItemType ItemType => _data.ItemType;
        public int Stack => _stackCount;
        public int MaxStack => _data.MaxStack;
        public float Weight => _stackCount * _data.Weight;
        public AItemRuntimeBase(ItemData itemData, int initialCount, Guid? runtimeGuid = null)
        {
            _data = itemData ?? throw new ArgumentNullException(nameof(itemData));
            _runtimeID = runtimeGuid ?? Guid.NewGuid();

            int count = (initialCount <= 0) ? 1 : initialCount;
            _stackCount = Math.Min(count, MaxStack);
        }

        /// <summary>
        /// スタックに追加する。実際に追加できた数を返す。
        /// </summary>
        public int AddToStack(int amount)
        {
            if (amount <= 0) return 0;

            int n = MaxStack - _stackCount;//上限まであとn個入れられるよ
            int toAdd = Math.Min(n, amount);
            _stackCount += toAdd;
            return toAdd;
        }

        /// <summary>
        /// スタックから指定数を取り除く。実際に減らせた数を返す。
        /// </summary> 
        public int ReduceStack(int amount)
        {
            if (amount <= 0) return 0;
            int reduced = Math.Min(_stackCount, amount);
            _stackCount -= reduced;
            return reduced;
        }

        public bool Merge(IItemRuntime other)
        {
            if (!IsSameType(other) || other == this) return false;

            int addNum = AddToStack(other.Stack);//足せたぶん
            other.ReduceStack(addNum);//足せたぶん減らす

            //otherのCountが0なら完全統合。残っていれば溢れパターン
            return other.Stack <= 0;
        }

        //アイテム生成はFactoryが管理する。
        //アイテム自身は「自分を複製する機能」を捨て、純粋に「自分の数を減らす」という責任だけを持つ。

        // public IItemRuntime Split(int amount)
        // {
        //     if(amount <= 0 || amount >= _stackCount) return null;

        //     IItemRuntime newItem = CreateCopy(amount);

        //     //生成が成功してはじめて数を減らす
        //     _stackCount -= amount;

        //     return newItem;
        // }

        public bool IsSameType(IItemRuntime other)
        {
            if (other == null) return false;

            //参照する静的データが同じかどうか
            return _data.ItemID == other.ItemData.ItemID;
        }

        //アイテム生成はFactoryが管理する。
        //protected abstract AItemRuntimeBase CreateCopy(int initialCount);
    }
}
