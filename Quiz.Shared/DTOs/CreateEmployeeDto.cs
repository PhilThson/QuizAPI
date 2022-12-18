using System;
using System.ComponentModel.DataAnnotations;
using Quiz.Data.Models;

namespace Quiz.Shared.DTOs
{
	public class CreateEmployeeDto : CreatePersonDto
	{
		[Range(2800.0, 10000.0)]
		public decimal Salary { get; set; }
		[Range(0, 500)]
		public int? DaysOfLeave { get; set; }
		public double? HourlyRate { get; set; }
		public double? Overtime { get; set; }
		[StringLength(11)]
		public string? PhoneNumber { get; set; }
		[StringLength(50)]
		public string? Email { get; set; }
		public byte? JobId { get; set; }
		public byte? PositionId { get; set; }
		[Required]
		public DateTime? DateOfEmployment { get; set; }
		public DateTime? EmploymentEndDate { get; set; }

		public static explicit operator Pracownik(CreateEmployeeDto createEmployee)
		{
			return new Pracownik
			{
				Imie = createEmployee.FirstName,
				DrugieImie = createEmployee.SecondName,
				Nazwisko = createEmployee.LastName,
				DataUrodzenia = createEmployee.DateOfBirth,
				MiejsceUrodzenia = createEmployee.BirthCity,
				Pesel = createEmployee.PersonalNumber,
				Pensja = createEmployee.Salary,
				DniUrlopu = createEmployee.DaysOfLeave,
				WymiarGodzinowy = createEmployee.HourlyRate,
				Nadgodziny = createEmployee.Overtime,
				NrTelefonu = createEmployee.PhoneNumber,
				Email = createEmployee.Email,
				EtatId = createEmployee.JobId,
				StanowiskoId = createEmployee.PositionId,
				DataZatrudnienia = createEmployee.DateOfEmployment.Value,
				DataKoncaZatrudnienia = createEmployee.EmploymentEndDate
			};
		}
	}
}

