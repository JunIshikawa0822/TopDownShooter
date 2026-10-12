using System.Collections.Generic;

namespace HTN
{
    public class Plan
    {
        private readonly Queue<PrimitiveTask> _tasks;

        public IEnumerable<PrimitiveTask> Tasks => _tasks;

        public Plan()
        {
            _tasks = new Queue<PrimitiveTask>();
        }

        public Plan(Plan other)
        {
            _tasks = new Queue<PrimitiveTask>(other.Tasks);
        }

        public void Clear()
        {
            _tasks.Clear();
        }

        public void Enqueue(PrimitiveTask task)
        {
            _tasks.Enqueue(task);
        }
    }
}
