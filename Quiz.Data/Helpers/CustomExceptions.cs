using System;
namespace Quiz.Data.Helpers
{
	public class DataNotFoundException : Exception
	{
		public DataNotFoundException()
			: base("Nie znaleziono danych dla wskazanych parametrów")
		{ }

		public DataNotFoundException(string message)
			: base(message)
		{ }
	}

	public class DataValidationException : Exception
	{
		public DataValidationException()
			: base("Przesłano niepoprawny model")
		{ }
		public DataValidationException(string message)
			: base(message)
		{ }
	}

	public class AlreadyExistsException : Exception
	{
		public AlreadyExistsException()
			: base("Istnieje już rekord o podanych parametrach")
		{ }

		public AlreadyExistsException(string message)
			: base(message)
		{ }
    }

	public class AuthenticationException : Exception
	{
		public AuthenticationException()
			: base("Nie udało się pobrać użytkownika")
		{ }

		public AuthenticationException(string message)
			: base(message)
		{ }
	}

	public class InactiveUserException : Exception
	{
		public InactiveUserException()
			: base("Użytkownik jest nieaktywny")
		{ }

		public InactiveUserException(string message)
			: base(message)
		{ }
	}
}