using System;
using Quiz.Data.Models;
using Quiz.Shared.DTOs;

namespace Quiz.Infrastructure.Helpers
{
	public static class MappingExtensions
	{
        public static void FillEmployeeModel(this Pracownik p, CreateEmployeeDto createEmployee)
        {
            p.Imie = createEmployee.FirstName;
            p.DrugieImie = createEmployee.SecondName;
            p.Nazwisko = createEmployee.LastName;
            p.Pesel = createEmployee.PersonalNumber;
            p.DataUrodzenia = createEmployee.DateOfBirth;
            p.MiejsceUrodzenia = createEmployee.PlaceOfBirth;
            p.Pensja = createEmployee.Salary;
            p.DniUrlopu = createEmployee.DaysOfLeave;
            p.WymiarGodzinowy = createEmployee.HourlyRate;
            p.Nadgodziny = createEmployee.Overtime;
            p.NrTelefonu = createEmployee.PhoneNumber;
            p.Email = createEmployee.Email;
            p.EtatId = createEmployee.JobId;
            p.StanowiskoId = createEmployee.PositionId;
            p.DataZatrudnienia = createEmployee.DateOfEmployment.Value;
            p.DataKoncaZatrudnienia = createEmployee.EmploymentEndDate;
        }

        public static void FillStudentModel(this Uczen u, CreateStudentDto createStudent)
        {
            u.Imie = createStudent.FirstName;
			u.DrugieImie = createStudent.SecondName;
			u.Nazwisko = createStudent.LastName;
			u.Pesel = createStudent.PersonalNumber;
			u.MiejsceUrodzenia = createStudent.PlaceOfBirth;
			u.DataUrodzenia = createStudent.DateOfBirth;
			u.NrOrzeczenia = createStudent.DisabilityCert;
			u.OddzialId = createStudent.BranchId.Value;
        }

        public static void FillAddressModel(this Adres a, AddressDto addressDto)
        {
            a.Panstwo = addressDto.Country;
            a.Miejscowosc = addressDto.City;
            a.Ulica = addressDto.Street;
            a.NumerDomu = addressDto.HouseNumber;
            a.NumerMieszkania = addressDto.FlatNumber;
            a.KodPocztowy = addressDto.PostalCode;
        }
    }
}

