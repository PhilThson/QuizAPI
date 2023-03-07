using System;
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

        public ActiveUserFilter(IDataService dataService)
		{
            _dataService = dataService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                var email = context.HttpContext.User.FindFirst("user")?.Value ??
                    throw new AuthenticationException();

                var user = await _dataService.GetUserByEmail(email);

                if (!user.IsActive)
                    throw new AuthenticationException("Użytkownik jest nieaktywny");
            }
            catch (Exception e)
            {
                context.Result = new UnauthorizedResult();
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

