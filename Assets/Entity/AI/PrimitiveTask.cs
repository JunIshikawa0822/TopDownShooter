using UnityEngine;
using System.Collections.Generic;

namespace HTN
{
    [CreateAssetMenu(fileName = "primitive_task.asset", menuName = "MyGame/AI/Primitive Task", order = 0)]
    public class PrimitiveTask : ATask
    {
        [SerializeReference, SubclassSelector] List<ATaskPreCondition> _preConditions;
        [SerializeField] ATaskOperation _operation;
        [SerializeReference] List<TaskEffect> _effects;

        public IReadOnlyList<ATaskPreCondition> PreConditions => _preConditions;
        public ATaskOperation Operation => _operation;
        public IReadOnlyList<TaskEffect> Effects => _effects;
        public override bool TryPlanTask(WorldState worldStateCopy, SelfState selfStateCopy, ref Plan plan)
        {
            foreach (ATaskPreCondition condition in _preConditions)
            {
                if (!condition.CanExecute(worldStateCopy, selfStateCopy))
                    return false;
            }

            foreach (TaskEffect effect in _effects)
                effect.ApplyTo(worldStateCopy, selfStateCopy);

            plan.Enqueue(this);

            return true;
        }
    }
}
