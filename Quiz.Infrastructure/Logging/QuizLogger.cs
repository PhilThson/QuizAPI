using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quiz.Data.Helpers;
using Quiz.Infrastructure.Interfaces;

namespace Quiz.Infrastructure.Logging
{
    public class QuizLogger : ILogger
    {
        private readonly string _connectionString;
        private readonly string _name;
        private readonly HttpContext? _httpContext;
        private readonly IBackgroundJobQueue _backgroundJobQueue;
        private readonly CancellationTokenSource _cts = new();

        public QuizLogger(string name,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IBackgroundJobQueue backgroundJobQueue)
        {
            _name = name;
            _connectionString = configuration.GetConnectionString("Default");
            _httpContext = httpContextAccessor.HttpContext;
            _backgroundJobQueue = backgroundJobQueue;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new QuizDisposable();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, 
            EventId eventId, 
            TState state, 
            Exception? exception, 
            Func<TState, Exception?, string> formatter
            )
        {
            if (eventId.Id != QuizConstants.EventId)
                return;

            var message = formatter(state, exception);
            var requestUrl = _httpContext?.Request.Path.Value;
            var requestType = _httpContext?.Request.Method;
            var token = _cts.Token;

            _backgroundJobQueue.Enqueue(async (token) =>
                await PerformLogging(logLevel, exception, message, _name, requestUrl, requestType, token));
        }

        private async Task PerformLogging(LogLevel logLevel, Exception? exception, 
            string message, string? logger, string? requestUrl, string? requestType,
            CancellationToken cancelToken)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = @"INSERT INTO Logi (Timestamp,Level,Message,Logger,RequestUrl,RequestType,Exception) 
                               VALUES (@timestamp, @level, @message, @logger, @requestUrl, @requestType, @exception)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@timestamp", DateTime.UtcNow);
                    var logLevelParam = command.Parameters.AddWithValue("@level", logLevel.ToString() ?? (object)DBNull.Value);
                    var messageParam = command.Parameters.AddWithValue("@message", message ?? (object)DBNull.Value);
                    var loggerParam = command.Parameters.AddWithValue("@logger", logger ?? (object)DBNull.Value);
                    var requestUrlParam = command.Parameters.AddWithValue("@requestUrl", requestUrl ?? (object)DBNull.Value);
                    var requestTypeParam = command.Parameters.AddWithValue("@requestType", requestType ?? (object)DBNull.Value);
                    var exceptionParam = command.Parameters.AddWithValue("@exception", exception?.ToString() ?? (object)DBNull.Value);

                    connection.Open();
                    await command.ExecuteNonQueryAsync(cancelToken);
                    connection.Close();
                }
            }
        }

        private class QuizDisposable : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}
