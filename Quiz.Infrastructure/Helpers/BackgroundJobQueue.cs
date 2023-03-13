using System.Collections.Concurrent;
using Quiz.Infrastructure.Interfaces;

namespace Quiz.Infrastructure.Helpers
{
    public class BackgroundJobQueue : IBackgroundJobQueue
    {
        private readonly ConcurrentQueue<Func<CancellationToken, Task>> _queue = new();

        public void Enqueue(Func<CancellationToken, Task> taskFactory)
        {
            _ = taskFactory ?? throw new ArgumentNullException(nameof(taskFactory));
            
            _queue.Enqueue(taskFactory);
        }

        public Func<CancellationToken, Task>? Dequeue()
        {
            if (_queue.TryDequeue(out var taskFactory))
                return taskFactory;

            return null;
        }
    }
}
