using System.Collections.Concurrent;
using Quiz.Infrastructure.Interfaces;

namespace Quiz.Infrastructure.Helpers
{
    public class BackgroundJobQueue : IBackgroundJobQueue
    {
        private readonly ConcurrentQueue<Action> _queue = new();

        public void Enqueue(Action action)
        {
            _ = action ?? throw new ArgumentNullException(nameof(action));
            
            _queue.Enqueue(action);
        }

        public Action? Dequeue()
        {
            if (_queue.TryDequeue(out var action))
                return action;

            return null;
        }
    }
}
