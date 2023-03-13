namespace Quiz.Infrastructure.Interfaces
{
    public interface IBackgroundJobQueue
    {
        void Enqueue(Func<CancellationToken, Task> taskFactory);
        Func<CancellationToken, Task>? Dequeue();
    }
}