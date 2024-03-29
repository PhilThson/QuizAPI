﻿using System;
using System.ComponentModel.DataAnnotations;
using Quiz.Data.Models;
using static Quiz.Data.Helpers.CommonExtensions;

namespace Quiz.Shared.DTOs
{
	public class CreateEmployeeDto : CreatePersonDto
	{
		[Range(0.0, 99999.0)]
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
        public List<int>? AddressesIds { get; set; }

        public static explicit operator Pracownik(CreateEmployeeDto createEmployee)
        {
            return new Pracownik
            {
                Imie = createEmployee.FirstName,
                DrugieImie = createEmployee.SecondName,
                Nazwisko = createEmployee.LastName,
                DataUrodzenia = createEmployee.DateOfBirth,
                MiejsceUrodzenia = createEmployee.PlaceOfBirth,
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
                DataKoncaZatrudnienia = createEmployee.EmploymentEndDate,
                CzyAktywny = true
            };
        }

        public static bool operator ==(Pracownik p, CreateEmployeeDto e)
        {
            if (p is null)
            {
                if (e is null)
                    return true;
                return false;
            }
            return
                p.Imie.ToLower() == e.FirstName.ToLower() &&
                SafeToLower(p.DrugieImie) == SafeToLower(e.SecondName) &&
                p.Nazwisko.ToLower() == e.LastName.ToLower() &&
                SafeToLower(p.MiejsceUrodzenia) == SafeToLower(e.PlaceOfBirth) &&
                p.DataUrodzenia == e.DateOfBirth &&
                p.Pesel == e.PersonalNumber &&
                p.NrTelefonu == e.PhoneNumber &&
                p.Email == e.Email &&
                p.EtatId == e.JobId &&
                p.StanowiskoId == e.PositionId &&
                p.DataZatrudnienia == e.DateOfEmployment.Value &&
                p.DataKoncaZatrudnienia == e.EmploymentEndDate
                ;
        }

        public static bool operator !=(Pracownik p, CreateEmployeeDto e) =>
            !(p == e);
    }
}

