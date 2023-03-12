﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quiz.Infrastructure.Interfaces;
using System.Collections.Concurrent;

namespace Quiz.Infrastructure.Logging
{
    public class QuizLoggerProvider : ILoggerProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBackgroundJobQueue _backgroundJobQueue;
        private readonly ConcurrentDictionary<string, QuizLogger> _loggers =
            new(StringComparer.OrdinalIgnoreCase);

        public QuizLoggerProvider(IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IBackgroundJobQueue backgroundJobQueue)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _backgroundJobQueue = backgroundJobQueue;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => 
                new QuizLogger(name,
                    _configuration,
                    _httpContextAccessor,
                    _backgroundJobQueue));
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}
