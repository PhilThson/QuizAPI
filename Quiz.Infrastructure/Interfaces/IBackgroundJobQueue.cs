namespace Quiz.Infrastructure.Interfaces
{
    public interface IBackgroundJobQueue
    {
        void Enqueue(Action action);
        Action? Dequeue();
    }
}