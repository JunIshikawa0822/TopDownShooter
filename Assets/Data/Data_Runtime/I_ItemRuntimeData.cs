using UnityEngine;
using System;

namespace Game.Items
{
    public interface IItemRuntimeData
    {
        ItemData BaseData { get; } // 元の ScriptableObject データ
        int StackCount { get; }    // 現在のスタック数
        int MaxStack { get; }      // スタック上限
        ItemType ItemType { get; } // アイテムの種類
        bool CanStack { get; }     // スタック可能か

        //スタックに追加する。追加できなかった余りを返す。
        int AddToStack(int amount);

        //スタックから指定数を取り除く（消費・分割両方で使用可能）。
        // 実際に減らせた数を返す。
        int ReduceStack(int amount);

        //別のスタックと結合する。結合できなかった余りを返す。
        bool Merge(IItemRuntimeData other);
    }
}