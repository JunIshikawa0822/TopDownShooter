using UnityEngine;
using System;

namespace Game.Data
{
    public interface IItemRuntimeData
    {
        string RuntimeID { get; }
        ItemData BaseData { get; } // 元の ScriptableObject データ
        ItemVisualData VisualData { get; }
        int StackCount { get; }//現在のスタック数
        float CurrentWeight{get;} //重量
        ItemType ItemType { get; }//アイテムの種類

        /// <summary>
        /// スタックに追加する。追加できなかった余りを返す。
        /// </summary>
        int AddToStack(int amount);

        /// <summary>
        /// スタックから指定数を取り除く（消費・分割両方で使用可能）。実際に減らせた数を返す。
        /// </summary>        
        int ReduceStack(int amount);
        
        /// <summary>
        /// 別のスタックと結合する。完全に結合できたらtrue できなかったらfalse
        /// </summary>
        bool Merge(IItemRuntimeData other);
    }
}