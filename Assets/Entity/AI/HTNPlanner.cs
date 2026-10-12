namespace HTN
{
    public class HTNPlanner
    {
        Plan _plan = new Plan();


        /// <summary>
        /// プランニングする
        /// </summary>
        /// <param name="rootTask">大元のタスク</param>
        public Plan DoPlan(WorldState worldState, SelfState selfState, ITask rootTask)
        {
            if (_plan == null) _plan = new Plan();
            _plan.Clear();

            // WorldStateのコピーを取って自由に書き換えられるようにする
            WorldState worldStateCopy = worldState.CreateCopy();
            SelfState selfStateCopy = selfState.CreateCopy();
            rootTask.TryPlanTask(worldStateCopy, selfStateCopy, ref _plan);

            return _plan;
        }
    }
}