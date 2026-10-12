using UnityEngine;
using System.Collections.Generic;

namespace HTN
{
    public class HTNPlanRunner : IOnUpdate
    {
        private readonly HTNPlanner _planner;
        private ITaskOperatable _operatable;
        private ITask _rootTask;
        private float _replanningInterval = 1f;

        //実行状態の管理
        private Queue<PrimitiveTask> _queuedTasks;
        private PrimitiveTask _currentTask;
        private ATaskOperator _currentOperator;
        private float _nextReplanningTime;

        private WorldState _worldState;
        private SelfState _selfState;

        public bool IsActiveForUpdate => _operatable != null;

        public HTNPlanRunner(WorldState worldState, ITaskOperatable operatable, ITask rootTask)
        {
            _operatable = operatable;
            _rootTask = rootTask;
            _worldState = worldState;
            _selfState = operatable.SelfState;
        }

        public void OnUpdate()
        {
            // 1. 定期的なリプランニングのチェック
            if (Time.time >= _nextReplanningTime)
            {
                Replanning();
                _nextReplanningTime = Time.time + _replanningInterval;
            }

            // 2. 現在進行中のアクションを実行
            UpdateExecution();
        }

        private void Replanning()
        {
            // 進行中のアクションを中断して再計画
            Plan plan = _planner.DoPlan(_worldState, _selfState, _rootTask);

            Debug.Log("プランニングしました");

            // プランをセット（既存の進行状態は破棄される）
            _queuedTasks = new Queue<PrimitiveTask>(plan.Tasks);
            _currentTask = null;
            _currentOperator = null;
        }

        private void UpdateExecution()
        {
            // 実行中のアクションがない場合、次のタスクを取り出す
            if (_currentOperator == null)
            {
                if (_queuedTasks == null || _queuedTasks.Count == 0)
                {
                    return; // 実行すべきタスクがない
                }

                _currentTask = _queuedTasks.Dequeue();
                _currentOperator = _currentTask.Operation.CreateOperator(_operatable);
            }

            // 1フレーム分の処理を実行
            _currentOperator.Tick(_worldState, _selfState);

            // 状態の確認
            if (_currentOperator.Status == ExecutionStatus.Failure)
            {
                Debug.LogWarning($"アクション失敗: {_currentTask.Operation.OperationName}。即座にリプランニングします。");
                Replanning();
            }
            else if (_currentOperator.Status == ExecutionStatus.Success)
            {
                OnTaskComplete();
            }
        }

        private void OnTaskComplete()
        {
            // Effectを現実のWorldStateに適用
            if (_currentTask != null)
            {
                foreach (TaskEffect effect in _currentTask.Effects)
                {
                    effect.ApplyTo(_worldState, _selfState);
                }
            }

            // 状態をリセットして次のUpdateで次のタスクへ映るようにする
            _currentTask = null;
            _currentOperator = null;
        }
    }
}
