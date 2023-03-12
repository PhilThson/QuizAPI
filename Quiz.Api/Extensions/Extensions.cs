using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;
using Quiz.Infrastructure.Helpers;

namespace Quiz.Api.Extensions
{
    public static class Extensions
    {
        public static ILoggingBuilder AddQuizLogger(this ILoggingBuilder builder)
        {
            builder.Services.TryAddEnumerable(
                ServiceDescriptor
                    .Singleton<ILoggerProvider, QuizLoggerProvider>());
            LoggerProviderOptions
                .RegisterProviderOptions<ConsoleLoggerOptions, QuizLoggerProvider>(builder.Services);

            return builder;
        }
    }
}
