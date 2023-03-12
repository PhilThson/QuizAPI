using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Quiz.Infrastructure.Helpers
{
    public class QuizLoggerProvider : ILoggerProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public QuizLoggerProvider(IConfiguration configuration, 
            IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }


        public ILogger CreateLogger(string categoryName)
        {
            return new QuizLogger(_configuration, _httpContextAccessor);
        }

        public void Dispose()
        {
            
        }
    }
}
