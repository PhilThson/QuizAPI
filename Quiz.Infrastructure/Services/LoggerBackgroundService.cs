using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quiz.Infrastructure.Interfaces;
using System.Diagnostics;

namespace Quiz.Infrastructure.Services
{
    public class LoggerBackgroundService : BackgroundService
    {
        private readonly ILogger<LoggerBackgroundService> _logger;
        private readonly IBackgroundJobQueue _jobQueue;

        public LoggerBackgroundService(
            ILogger<LoggerBackgroundService> logger, 
            IBackgroundJobQueue jobQueue)
        {
            _logger = logger;
            _jobQueue = jobQueue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var job = _jobQueue.Dequeue();

                if (job != null)
                {
                    try
                    {
                        await job(stoppingToken);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Niepowodzenie w trakcie przetwarzania żądania. '{e}'", e.Message);
                        _logger.LogError(e, "Wystąpił błąd wykonywania zadania w tle");
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
            }
        }
    }
}
