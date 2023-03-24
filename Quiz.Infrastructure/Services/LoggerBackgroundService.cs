using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quiz.Infrastructure.Interfaces;

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
                try
                {
                    if (job is not null)
                    {
                        job();
                    }
                }
                catch (Exception e)
                {
                    //tutaj nie będzie zapętlenia, z racji na to, że customowy logger, loguje
                    //tylko zdarzenia o zdefiniowanym wcześniej Id
                    _logger.LogError(e, "Niepowodzenie w trakcie przetwarzania żądania w tle.");
                }

                await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
            }
        }
    }
}
