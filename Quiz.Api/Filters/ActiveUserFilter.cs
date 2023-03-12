using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Quiz.Data.Helpers;
using Quiz.Infrastructure.Interfaces;

namespace Quiz.Api.Filters
{
    public class ActiveUserFilter : AuthorizeAttribute, IAsyncAuthorizationFilter
	{
        private readonly IDataService _dataService;
        private readonly ILogger<ActiveUserFilter> _logger;

        public ActiveUserFilter(IDataService dataService, 
            ILogger<ActiveUserFilter> logger)
        {
            _dataService = dataService;
            _logger = logger;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                var email = context.HttpContext.User.FindFirst("user")?.Value ??
                    throw new AuthenticationException("Brak zalogowanego użytkownika");

                var user = await _dataService.GetUserByEmail(email);

                if (!user.IsActive)
                    throw new AuthenticationException($"Użytkownik '{user.Email}' jest nieaktywny");
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);

                context.Result = new ObjectResult(e.Message)
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
        }
    }

    public class ActiveUserAttribute : TypeFilterAttribute
    {
        public ActiveUserAttribute() : base(typeof(ActiveUserFilter))
        {

        }
    }
}

