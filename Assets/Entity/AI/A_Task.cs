using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HTN
{
    public abstract class ATask : ScriptableObject, ITask
    {
        public abstract bool TryPlanTask(WorldState worldState, SelfState selfState, ref Plan plan);
    }
}
