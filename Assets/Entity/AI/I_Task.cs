using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HTN
{
    public interface ITask
    {
        bool TryPlanTask(WorldState state, SelfState selfState, ref Plan plan);
    }
}
