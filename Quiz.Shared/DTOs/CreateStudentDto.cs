using System;
using System.ComponentModel.DataAnnotations;
using Quiz.Data.Models;

namespace Quiz.Shared.DTOs
{
	public class CreateStudentDto : CreatePersonDto
	{
		[Required]
		[StringLength(15)]
        public string DisabilityCert { get; set; }
        [Required]
		public byte? BranchId { get; set; }

		public static explicit operator Uczen(CreateStudentDto createStudent) =>
			new Uczen
			{
				Imie = createStudent.FirstName,
				DrugieImie = createStudent.SecondName,
				Nazwisko = createStudent.LastName,
				Pesel = createStudent.PersonalNumber,
				MiejsceUrodzenia = createStudent.PlaceOfBirth,
				DataUrodzenia = createStudent.DateOfBirth,
				NrOrzeczenia = createStudent.DisabilityCert,
				OddzialId = createStudent.BranchId.Value,
				CzyAktywny = true
			};
    }
}

