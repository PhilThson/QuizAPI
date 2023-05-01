namespace Quiz.Infrastructure.Interfaces
{
    public interface ILoggerAdapter<T>
    {
        void LogInformation(string message);

        void LogInformation<T0>(string message, T0 arg0);

        void LogWarning(string message);

        void LogWarning<T0>(string message, T0 arg0);

        void LogWarning<T0, T1>(string message, T0 arg0, T1 arg1);

        void LogError(Exception? e, string message);

        void LogError<T0>(Exception? e, string message, T0 arg0);

        void LogError<T0, T1>(Exception? e, string message, T0 arg0, T1 arg1);
    }
}
