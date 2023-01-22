using System;
using System.ComponentModel.DataAnnotations;
using Quiz.Data.Models;
using static Quiz.Data.Helpers.CommonExtensions;

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

		public static bool operator ==(Uczen u, CreateStudentDto s)
		{
			if(u is null)
			{
				if (s is null)
					return true;
				return false;
			}
			return
				u.Imie.ToLower() == s.FirstName.ToLower() &&
				SafeToLower(u.DrugieImie) == SafeToLower(s.SecondName) &&
				u.Nazwisko.ToLower() == s.LastName.ToLower() &&
				SafeToLower(u.MiejsceUrodzenia) == SafeToLower(s.PlaceOfBirth) &&
				u.DataUrodzenia == s.DateOfBirth &&
				u.Pesel == s.PersonalNumber &&
				SafeToLower(u.NrOrzeczenia) == SafeToLower(s.DisabilityCert) &&
				u.OddzialId == s.BranchId;
        }

		public static bool operator !=(Uczen u, CreateStudentDto s) =>
			!(u == s);
    }
}

