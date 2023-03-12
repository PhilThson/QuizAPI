namespace Quiz.Infrastructure.Interfaces
{
    public interface ILogAdapter<T>
    {
        void LogError(string message);
        void LogError<T0, T1>(string message, T0 arg0, T1 arg1);
        void LogError<TE, T0>(TE e, string message, T0 arg0)
            where TE : Exception;
        void LogError<T0>(string message, T0 arg0);
        void LogInformation(string message);
        void LogInformation<T0, T1>(string message, T0 arg0, T1 arg1);
        void LogInformation<T0>(string message, T0 arg0);
        void LogWarning(string message);
        void LogWarning<T0, T1>(string message, T0 arg0, T1 arg1);
        void LogWarning<TE, T0>(TE e, string message, T0 arg0)
            where TE : Exception;
        void LogWarning<T0>(string message, T0 arg0);
    }
}