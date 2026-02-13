using UnityEngine;

namespace HTN
{
    public abstract class ATaskOperation : ScriptableObject, ITaskOperation
    {
        [SerializeField] string _operationName;
        public string OperationName => _operationName;
        public abstract ATaskOperator CreateOperator(ITaskOperatable operatable);
    }
}
