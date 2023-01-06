using System;
namespace Quiz.Shared.DTOs
{
	public class CreateDiagnosisDto
	{
		public int EmployeeId { get; set; }
		public int StudentId { get; set; }
		public string SchoolYear { get; set; }
		public byte DifficultyId { get; set; }
	}
}

