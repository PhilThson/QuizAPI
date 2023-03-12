using System;
using Quiz.Data.Helpers;

namespace Quiz.Api.CustomMiddleware
{
    public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;
		private static string defualtMessage = "Błąd przetwarzania żądania.";

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
				context.Response.StatusCode = e switch
				{
					DataValidationException or AlreadyExistsException => StatusCodes.Status400BadRequest,
					AuthenticationException => StatusCodes.Status401Unauthorized,
					DataNotFoundException => StatusCodes.Status404NotFound,
					_ => StatusCodes.Status500InternalServerError,
				};

				await context.Response.WriteAsync(e.Message);
				_logger.LogWarning(e, "{message}", e.Message);
			}
		}
	}
}

