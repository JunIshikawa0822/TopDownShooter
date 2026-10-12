// 設定（設計図）
namespace HTN
{
    public interface ITaskOperation
    {
        string OperationName { get; }
        ATaskOperator CreateOperator(ITaskOperatable operatable);
    }
}