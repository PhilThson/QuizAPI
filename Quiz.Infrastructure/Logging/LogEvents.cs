using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quiz.Data.Helpers;
using Quiz.Infrastructure.Helpers;

namespace Quiz.Infrastructure.Logging
{
    public static partial class LogEvents
    {
        [LoggerMessage(EventId = QuizConstants.EventId, 
            Level = LogLevel.Information,
            Message = "{message}",
            SkipEnabledCheck = true)]
        public static partial void Info(this ILogger logger, string message);

        //[LoggerMessage(EventId = QuizConstants.EventId,
        //    Level = LogLevel.Warning,
        //    Message = "{message}",
        //    SkipEnabledCheck = true)]
        //public static partial void Warn(this ILogger logger, string message);

        //[LoggerMessage(EventId = QuizConstants.EventId,
        //    Level = LogLevel.Error,
        //    Message = "{message}",
        //    SkipEnabledCheck = true)]
        //public static partial void Error(this ILogger logger, string message);
    }

    public static class LogDefinitions
    {
        private static readonly Action<ILogger, string, Exception?> WarningMessageDefinition =
            LoggerMessage.Define<string>(LogLevel.Warning, 
                new EventId(QuizConstants.EventId),
                "{LogMessage}");

        public static void Warn(this ILogger logger, LogMessage logMessage)
        {
            WarningMessageDefinition(logger, JsonConvert.SerializeObject(logMessage), default!);
        }
    }
}
