using System;
using System.ComponentModel.DataAnnotations;
using Quiz.Data.Models;
using static Quiz.Data.Helpers.CommonExtensions;

namespace Quiz.Shared.DTOs
{
	public class AddressDto
	{
		public int Id { get; set; }

		[Required]
		[StringLength(64)]
		public string Country { get; set; }

		[Required]
		[StringLength(128)]
		public string City { get; set; }

        [StringLength(128)]
        public string? Street { get; set; }

        [Required]
        [StringLength(10)]
        public string HouseNumber { get; set; }

        [StringLength(10)]
        public string? FlatNumber { get; set; }

        [Required]
        [StringLength(10)]
        public string PostalCode { get; set; }

        public static bool operator==(Adres a, AddressDto addressDto)
        {
            if(a is null)
            {
                if (addressDto is null)
                    return true;
                return false;
            }

            return
                a.Panstwo.ToLower() == addressDto.Country.ToLower() &&
                a.Miejscowosc.ToLower() == addressDto.City.ToLower() &&
                SafeToLower(a.Ulica) == SafeToLower(addressDto.Street) &&
                a.NumerDomu.ToLower() == addressDto.HouseNumber.ToLower() &&
                SafeToLower(a.NumerMieszkania) == SafeToLower(addressDto.FlatNumber) &&
                a.KodPocztowy.ToLower() == addressDto.PostalCode;
        }

        public static bool operator !=(Adres a, AddressDto addressDto) =>
            !(a == addressDto);

        public static explicit operator Adres(AddressDto addressDto) =>
            new Adres()
            {
                Panstwo = addressDto.Country,
                Miejscowosc = addressDto.City,
                Ulica = addressDto.Street,
                NumerDomu = addressDto.HouseNumber,
                NumerMieszkania = addressDto.FlatNumber,
                KodPocztowy = addressDto.PostalCode,
                CzyAktywny = true
            };
	}
}

