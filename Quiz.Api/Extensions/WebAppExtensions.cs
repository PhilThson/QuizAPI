using System;
using Quiz.Api.CustomMiddleware;

namespace Quiz.Api.Extensions
{
	public static class WebAppExtensions
	{
		public static void HandleExceptions(this WebApplication webApp)
		{
			webApp.UseMiddleware<ExceptionMiddleware>();
		}
	}
}

