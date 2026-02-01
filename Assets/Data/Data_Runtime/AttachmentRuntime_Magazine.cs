using UnityEngine;
using System.Collections.Generic;

namespace Game.Data
{
    public class AttachmentRuntime_Magazine : AttachmentRuntime
    {
        private uint _remaining;
        private AmmoData _loadedAmmoData;

        public AttachmentData_Magazine MagazineData => AttachmentData as AttachmentData_Magazine;
        public uint Remaining => _remaining;
        public AmmoData LoadedAmmoData => _loadedAmmoData;

        public AttachmentRuntime_Magazine(AttachmentData_Magazine magazineData, int initialCount = 1) : base(magazineData, initialCount)
        {
            
        }

        public bool CanLoad(IItemRuntime item)
        {
            if(item == null) return false;
            if(!(item is AmmoRuntime ammo)) return false;//そもそも弾薬か
            if(ammo.AmmoData == null || ammo.AmmoType != MagazineData.TargetAmmo) return false;//マッチする口径か
            if(_remaining > 0 && _loadedAmmoData != null) return false;//すでに違う種類の弾が入っている
            if (_remaining >= MagazineData.Capacity)return false;//もう弾が入らない

            return true;
        }
        
        /// <summary>
        /// 装填の実行
        /// </summary>
        /// <param name="item">装填しようとしているアイテム</param>
        /// <param name="consumeSource">手持ちの弾を実際に減らすかどうか（拠点ならfalse）</param>
        public bool TryLoad(IItemRuntime item, bool consumeSource = true)
        {
            //CanLoadでの互換性チェックはこれまで通り（型が違うなら入れられない）
            if (!CanLoad(item)) return false;

            AmmoRuntime ammoStack = (AmmoRuntime)item;
            
            // マガジンの空き容量を計算
            uint space = MagazineData.Capacity - _remaining;
            if (space <= 0) return false;

            // 装填する数を決定
            // 無限リロードモードなら、手持ちの数に関わらず「空き容量分」をフル補充できる
            int toLoad = consumeSource ? Mathf.Min((int)space, ammoStack.Stack) : (int)space;

            _loadedAmmoData = ammoStack.AmmoData;
            _remaining += (uint)toLoad;

            // ここがポイント：consumeSourceがtrueの時だけ、インベントリの弾を減らす
            if (consumeSource) ammoStack.ReduceStack(toLoad);

            return true;
        }

        //TODO: UnLoadの仕組みを作る際、だれがAmmoRuntimeを作成するのか問題
        // public AmmoRuntime UnLoad()
        // {
            
        // }

        /// <summary>
        /// 発射時の消費処理
        /// </summary>
        /// <param name="consumeRemaining">マガジン内の弾を減らすかどうか（拠点でも射撃感覚のために減らすならtrue）</param>
        public bool Consume(bool consumeRemaining = true)
        {
            if (_remaining <= 0) return false;

            if (consumeRemaining)
            {
                _remaining--;
            }

            if (_remaining <= 0)
            {
                _loadedAmmoData = null;
            }

            return true;
        }
    }
}
