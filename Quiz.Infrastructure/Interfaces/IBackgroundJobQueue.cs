namespace Quiz.Infrastructure.Interfaces
{
    public interface IBackgroundJobQueue
    {
        void Enqueue(Func<CancellationToken, Task> taskFactory);
        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}