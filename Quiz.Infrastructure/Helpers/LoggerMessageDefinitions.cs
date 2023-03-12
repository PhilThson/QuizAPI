using Microsoft.Extensions.Logging;

namespace Quiz.Infrastructure.Helpers
{
    public static partial class LoggerMessageDefinitions
    {
        [LoggerMessage(EventId = 0, Level = LogLevel.Information,
            Message = "Message with parameters {First}, {Second}",
            SkipEnabledCheck = true)]
        public static partial void LogQuizMessage(this ILogger logger, int first, int second);
    }
}
