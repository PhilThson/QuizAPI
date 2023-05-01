using Microsoft.Extensions.Logging;
using Quiz.Data.Helpers;
using Quiz.Infrastructure.Interfaces;

namespace Quiz.Infrastructure.Logging
{
    public class LoggerAdapter<T> : ILoggerAdapter<T>
    {
        private readonly ILogger _logger;

        public LoggerAdapter(ILogger logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message)
        {
            if (_logger.IsEnabled(LogLevel.Information))
                _logger.LogInformation(QuizConstants.EventId, message);
        }

        public void LogInformation<T0>(string message, T0 arg0)
        {
            if (_logger.IsEnabled(LogLevel.Information))
                _logger.LogInformation(QuizConstants.EventId, message, arg0);
        }

        public void LogWarning(string message)
        {
            if (_logger.IsEnabled(LogLevel.Warning))
                _logger.LogWarning(QuizConstants.EventId, message);
        }

        public void LogWarning<T0>(string message, T0 arg0)
        {
            if (_logger.IsEnabled(LogLevel.Warning))
                _logger.LogWarning(QuizConstants.EventId, message, arg0);
        }

        public void LogWarning<T0, T1>(string message, T0 arg0, T1 arg1)
        {
            if (_logger.IsEnabled(LogLevel.Warning))
                _logger.LogWarning(QuizConstants.EventId, message, arg0, arg1);
        }

        public void LogError(Exception? e, string message)
        {
            if (_logger.IsEnabled(LogLevel.Error))
                _logger.LogError(QuizConstants.EventId, e, message);
        }

        public void LogError<T0>(Exception? e, string message, T0 arg0)
        {
            if (_logger.IsEnabled(LogLevel.Error))
                _logger.LogError(QuizConstants.EventId, e, message, arg0);
        }

        public void LogError<T0, T1>(Exception? e, string message, T0 arg0, T1 arg1)
        {
            if (_logger.IsEnabled(LogLevel.Error))
                _logger.LogError(QuizConstants.EventId, e, message, arg0, arg1);
        }
    }
}
