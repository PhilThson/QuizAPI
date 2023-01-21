using System;
namespace Quiz.Data.Helpers
{
	public static class CommonExtensions
	{
		public static string SafeToLower(object obj) =>
			obj == null ? string.Empty : obj.ToString().ToLower();
	}
}

