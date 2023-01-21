using System.ComponentModel.DataAnnotations;
using Quiz.Data.Models;
using static Quiz.Data.Helpers.CommonExtensions;

namespace Quiz.Shared.DTOs
{
    public class CreateDictionaryDto
	{
		public byte Id { get; set; }
		[Required]
		[StringLength(512)]
		public string Name { get; set; }
		[StringLength(1024)]
		public string? Description { get; set; }
		[StringLength(1024)]
		public string? ExtendedName { get; set; }

		public static bool operator ==(SkalaTrudnosci s, CreateDictionaryDto c)
		{
			if(s == null)
			{
				if (c == null)
					return true;
				return false;
			}
			return
				s.Nazwa.ToLower() == c.Name.ToLower() &&
				SafeToLower(s.Opis) == SafeToLower(c.Description);
		}

        public static bool operator !=(SkalaTrudnosci s, CreateDictionaryDto c)
        {
			return !(s == c);
        }
    }
}

