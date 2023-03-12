using Microsoft.Extensions.Logging;
using Quiz.Data.Helpers;

namespace Quiz.Infrastructure.Logging
{
    public static partial class LogEvents
    {
        [LoggerMessage(EventId = QuizConstants.EventId, 
            Level = LogLevel.Information,
            Message = "{message}",
            SkipEnabledCheck = true)]
        public static partial void Info(this ILogger logger, string message);

        [LoggerMessage(EventId = QuizConstants.EventId,
            Level = LogLevel.Warning,
            Message = "{message}",
            SkipEnabledCheck = true)]
        public static partial void Warn(this ILogger logger, string message);

        [LoggerMessage(EventId = QuizConstants.EventId,
            Level = LogLevel.Error,
            Message = "{message}",
            SkipEnabledCheck = true)]
        public static partial void Error(this ILogger logger, string message);
    }
}
