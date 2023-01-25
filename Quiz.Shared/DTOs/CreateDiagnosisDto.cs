using System;
using System.ComponentModel.DataAnnotations;
using Quiz.Data.Models;

namespace Quiz.Shared.DTOs
{
	public class CreateDiagnosisDto
	{
        public string Institution { get; set; }
		public string SchoolYear { get; set; }
        public string CounselingCenter { get; set; }
		public int StudentId { get; set; }
		public int EmployeeId { get; set; }
		public byte DifficultyId { get; set; }

		public static explicit operator Diagnoza(CreateDiagnosisDto diagnosisDto) =>
			new Diagnoza
			{
                PracownikId = diagnosisDto.EmployeeId,
                UczenId = diagnosisDto.StudentId,
                RokSzkolny = diagnosisDto.SchoolYear,
                SkalaTrudnosciId = diagnosisDto.DifficultyId,
                PlacowkaOswiatowa = diagnosisDto.Institution,
                PoradniaPsychologiczna = diagnosisDto.CounselingCenter,
                CzyAktywny = true
            };
    }
}