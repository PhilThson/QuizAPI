using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quiz.Data.Helpers;
using Quiz.Infrastructure.Interfaces;
using Quiz.Infrastructure.Helpers;

namespace Quiz.Infrastructure.Logging
{
    public class QuizLogger : ILogger
    {
        private readonly string _connectionString;
        private readonly string _name;
        private readonly IBackgroundJobQueue _backgroundJobQueue;

        public QuizLogger(string name,
            IConfiguration configuration,
            IBackgroundJobQueue backgroundJobQueue)
        {
            _name = name;
            _connectionString = configuration.GetConnectionString("Default");
            _backgroundJobQueue = backgroundJobQueue;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

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
            var logMessage = new LogMessage();
            if (logLevel == LogLevel.Warning && !string.IsNullOrEmpty(eventId.Name))
            {
                logMessage = JsonConvert.DeserializeObject<LogMessage>(eventId.Name);
                if (logMessage != null)
                    message = logMessage.Message ?? message;
            }

            _backgroundJobQueue.Enqueue(() =>
                PerformLogging(logLevel, exception, message, _name, logMessage.RequestUrl, logMessage.RequestType));
        }

        private void PerformLogging(LogLevel logLevel, Exception? exception, 
            string message, string? logger, string? requestUrl, string? requestType)
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
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
