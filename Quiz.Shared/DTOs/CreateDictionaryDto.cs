using System;
using System.ComponentModel.DataAnnotations;

namespace Quiz.Shared.DTOs
{
	public class CreateDictionaryDto
	{
		[Required]
		[StringLength(512)]
		public string Name { get; set; }
		[StringLength(1024)]
		public string? Description { get; set; }
	}
}

