using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Quiz.Infrastructure.Helpers
{
    public class QuizLogger : ILogger
    {
        private readonly string _connectionString;
        private readonly HttpContext? _httpContext;

        public QuizLogger(IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = configuration.GetConnectionString("Default");
            _httpContext = httpContextAccessor.HttpContext;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new QuizDisposable();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (eventId.Id != 0)
                return;

            if (!string.IsNullOrEmpty(eventId.Name) &&
                eventId.Name.StartsWith("Executing"))
                return;

            var message = formatter(state, exception);
            var logger = state?.GetType().FullName;
            var requestUrl = _httpContext?.Request.Path.Value;
            var requestType = _httpContext?.Request.Method;

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
                    command.ExecuteNonQuery();
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
