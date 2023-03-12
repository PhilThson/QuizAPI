using Microsoft.Extensions.Logging;
using Quiz.Infrastructure.Interfaces;

namespace Quiz.Infrastructure.Helpers
{
    public class LogAdapter<T> : ILogAdapter<T>
    {
        #region Private fields
        private readonly ILogger<T> _logger;
        #endregion

        #region Constructor
        public LogAdapter(ILogger<T> logger)
        {
            _logger = logger;
        }
        #endregion

        #region Methods

        #region Information
        public void LogInformation(string message)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(message);
            }
        }

        public void LogInformation<T0>(string message, T0 arg0)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(message, arg0);
            }
        }

        public void LogInformation<T0, T1>(string message, T0 arg0, T1 arg1)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(message, arg0, arg1);
            }
        }
        #endregion

        #region Warning
        public void LogWarning(string message)
        {
            if (_logger.IsEnabled(LogLevel.Warning))
            {
                _logger.LogWarning(message);
            }
        }

        public void LogWarning<T0>(string message, T0 arg0)
        {
            if (_logger.IsEnabled(LogLevel.Warning))
            {
                _logger.LogWarning(message, arg0);
            }
        }

        public void LogWarning<T0, T1>(string message, T0 arg0, T1 arg1)
        {
            if (_logger.IsEnabled(LogLevel.Warning))
            {
                _logger.LogWarning(message, arg0, arg1);
            }
        }

        public void LogWarning<TE, T0>(TE e, string message, T0 arg0)
            where TE : Exception
        {
            if (_logger.IsEnabled(LogLevel.Warning))
            {
                _logger.LogWarning(e, message, arg0);
            }
        }
        #endregion

        #region Error
        public void LogError(string message)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(message);
            }
        }

        public void LogError<T0>(string message, T0 arg0)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(message, arg0);
            }
        }

        public void LogError<T0, T1>(string message, T0 arg0, T1 arg1)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(message, arg0, arg1);
            }
        }

        public void LogError<TE, T0>(TE e, string message, T0 arg0)
            where TE : Exception
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(e, message, arg0);
            }
        }
        #endregion

        #endregion
    }
}
