// 実行状態を保持する実体
namespace HTN
{
    public abstract class ATaskOperator
    {
        protected ExecutionStatus _status = ExecutionStatus.Running;
        public ExecutionStatus Status => _status;

        /// <summary>
        /// 実行状態を更新する あくまでStateは読み取り専用
        /// </summary>
        public abstract void Tick(WorldState worldState, SelfState selfState);
    }
}