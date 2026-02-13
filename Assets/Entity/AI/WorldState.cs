using UnityEngine;

namespace HTN
{
    [System.Serializable]
    public class WorldState
    {
        public WorldState()
        {

        }

        /// <summary>
        /// 自身を複製する
        /// </summary>
        /// <returns>自身の複製</returns>
        public WorldState CreateCopy()
        {
            WorldState copy = new WorldState();
            copy.CopyFrom(this);
            return copy;
        }

        public void CopyFrom(WorldState other)
        {
            //TODO: 全てのステータスをコピーする
        }

        public float GetFloatVariable(string variableName)
        {
            return 0;
        }
    }
}
