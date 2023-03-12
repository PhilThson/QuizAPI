using Quiz.Data.Helpers;
using Quiz.Infrastructure.Logging;

namespace Quiz.Api.CustomMiddleware
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;
		private readonly string _defualtMessage = "Wystąpił błąd wewnętrzny aplikacji.";

		public ExceptionMiddleware(RequestDelegate next, 
			ILogger<ExceptionMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
            try
			{
				await _next(context);
			}
			catch(Exception e)
			{
				var message = string.IsNullOrEmpty(e.Message) ? _defualtMessage : e.Message;

                context.Response.StatusCode = e switch
				{
					DataValidationException or 
					AlreadyExistsException => StatusCodes.Status400BadRequest,
					AuthenticationException => StatusCodes.Status401Unauthorized,
					DataNotFoundException => StatusCodes.Status404NotFound,
					_ => StatusCodes.Status500InternalServerError,
				};

				await context.Response.WriteAsync(message);
				_logger.Warn(message);
			}
		}
	}
}

