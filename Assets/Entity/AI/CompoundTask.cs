using System.Collections.Generic;
using UnityEngine;

namespace HTN
{
    [CreateAssetMenu(fileName = "compound_task.asset", menuName = "MyGame/AI/Compound Task", order = 0)]
    public class CompoundTask : ATask
    {
        [SerializeField] private List<Method> _methods;

        public override bool TryPlanTask(WorldState worldState, SelfState selfState, ref Plan plan)
        {
            foreach (Method method in _methods)
            {
                //そもそもMethodが実行可能か確かめる
                if (!method.CanExecute(worldState, selfState))
                {
                    continue;
                }

                //Plan適応時に問題が起こった場合、戻すためのコピー
                //現在のplanを保存
                Plan currentPlan = new Plan(plan);
                WorldState currentWorldState = worldState.CreateCopy();
                SelfState currentSelfState = selfState.CreateCopy();

                //サブタスクをシミュレーションしてみて、適応できるか確かめる
                if (!method.TryPlanSubTasks(worldState, selfState, ref plan))
                {
                    //失敗したときplan、WorldState、SelfStateを元に戻す
                    plan = currentPlan;
                    worldState.CopyFrom(currentWorldState);
                    selfState.CopyFrom(currentSelfState);
                    continue;
                }

                //サブタスクが全て完了したので成功
                return true;
            }

            //条件を満たすメソッドがなかったので失敗
            return false;
        }
    }
}