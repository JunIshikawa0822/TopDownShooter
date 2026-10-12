using System;
namespace Game.Data
{
    public interface IItemRuntime
    {
        Guid RuntimeID { get; }
        ItemData ItemData { get; }
        ItemVisualData VisualData { get; }
        int Stack { get; }//現在のスタック数
        int MaxStack { get; }//最大スタック数
        float Weight { get; } //重量
        ItemType ItemType { get; }//アイテムの種類

        /// <summary>
        /// スタックに追加する。実際に追加できた数を返す。
        /// </summary>
        /// <param name="amount">足したい量</param>
        /// <returns></returns>
        int AddToStack(int amount);

        /// <summary>
        /// スタックから指定数を取り除く。実際に減らせた数を返す。
        /// </summary>      
        /// <param name="amount">引きたい量</param>
        /// <returns></returns>  
        int ReduceStack(int amount);

        /// <summary>
        /// 別のスタックと結合する。完全に結合できたらtrue できなかったらfalse
        /// </summary>
        bool Merge(IItemRuntime other);

        /// <summary>
        /// 同種のアイテムであるかを調べる。同一性は無視。
        /// </summary>
        bool IsSameType(IItemRuntime other);
    }
}