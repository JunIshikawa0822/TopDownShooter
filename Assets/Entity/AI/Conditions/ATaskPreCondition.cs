using UnityEngine;

namespace HTN
{
    [System.Serializable]
    public abstract class ATaskPreCondition
    {
        //TODO: 実行可能条件を実装する
        public abstract bool CanExecute(WorldState state, SelfState selfState);
    }
}
