using System.Collections.Generic;
using UnityEngine;

namespace HTN
{
    [System.Serializable]
    public class Method
    {
        [SerializeReference] List<ATaskPreCondition> _preConditions;
        [SerializeField] List<ITask> _subTasks;

        public bool CanExecute(WorldState worldState, SelfState selfState)
        {
            foreach (ATaskPreCondition preCondition in _preConditions)
            {
                if (!preCondition.CanExecute(worldState, selfState))
                    return false;
            }

            return true;
        }

        public bool TryPlanSubTasks(WorldState worldState, SelfState selfState, ref Plan plan)
        {
            foreach (ITask task in _subTasks)
            {
                if (!task.TryPlanTask(worldState, selfState, ref plan))
                {
                    return false;
                }
            }

            return true;
        }
    }
}