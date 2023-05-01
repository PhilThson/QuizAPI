using Newtonsoft.Json;
using Quiz.Data.Helpers;
using Quiz.Infrastructure.Helpers;

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
                context.Response.StatusCode = e switch
				{
					DataValidationException or 
					AlreadyExistsException => StatusCodes.Status400BadRequest,
					AuthenticationException => StatusCodes.Status401Unauthorized,
					InactiveUserException => StatusCodes.Status403Forbidden,
					DataNotFoundException => StatusCodes.Status404NotFound,
					_ => StatusCodes.Status500InternalServerError,
				};

				var logMessage = new LogMessage
				{
					Message = string.IsNullOrEmpty(e.Message) ? 
						_defualtMessage : e.Message,
					RequestUrl = context?.Request.Path.Value,
					RequestType = context?.Request.Method
				};

				_logger.Log(LogLevel.Warning, new EventId(QuizConstants.EventId, 
					JsonConvert.SerializeObject(logMessage)), null);

				await context.Response.WriteAsync(logMessage.Message);
			}
		}
	}
}

