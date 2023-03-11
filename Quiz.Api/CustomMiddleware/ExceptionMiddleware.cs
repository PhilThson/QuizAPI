using System;
using Quiz.Data.Helpers;

namespace Quiz.Api.CustomMiddleware
{
    public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private static string defualtMessage = "Błąd przetwarzania żądania.";

		public ExceptionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

        public async Task InvokeAsync(HttpContext context)
		{
            try
			{
				await _next(context);
			}
			catch(Exception e)
			{
				switch(e)
				{
					case DataValidationException:
					case AlreadyExistsException:
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
						break;
					case AuthenticationException:
						context.Response.StatusCode = StatusCodes.Status401Unauthorized;
						break;
                    case DataNotFoundException:
						context.Response.StatusCode = StatusCodes.Status404NotFound;
						break;
					default:
						context.Response.StatusCode = StatusCodes.Status500InternalServerError;
						break;
				}

				await context.Response.WriteAsync(e.Message);
			}
		}
	}
}

