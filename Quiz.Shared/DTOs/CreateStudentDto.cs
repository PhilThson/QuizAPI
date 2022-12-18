using System;
using System.ComponentModel.DataAnnotations;
using Quiz.Data.Models;

namespace Quiz.Shared.DTOs
{
	public class CreateStudentDto : CreatePersonDto
	{
		[Required]
		public byte? BranchId { get; set; }

		public static explicit operator Uczen(CreateStudentDto createStudent) =>
			new Uczen
			{
				Imie = createStudent.FirstName,
				DrugieImie = createStudent.SecondName,
				Nazwisko = createStudent.LastName,
				Pesel = createStudent.PersonalNumber,
				MiejsceUrodzenia = createStudent.BirthCity,
				DataUrodzenia = createStudent.DateOfBirth,
				OddzialId = createStudent.BranchId.Value
			};
    }
}

