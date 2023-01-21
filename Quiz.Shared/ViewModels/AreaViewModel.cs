using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Quiz.Data.Models;
using static Quiz.Data.Helpers.CommonExtensions;

namespace Quiz.Shared.ViewModels
{
	public class AreaViewModel
	{
        public byte Id { get; set; }
        [Required]
        [MaxLength(512)]
        [DisplayName("Nazwa")]
        public string Name { get; set; }
        [MaxLength(1024)]
        [DisplayName("Opis")]
        public string? Description { get; set; }
        [MaxLength(1024)]
        [DisplayName("Nazwa rozszerzona")]
        public string? ExtendedName { get; set; }

        public static bool operator ==(ObszarZestawuPytan a1, AreaViewModel a2)
        {
            if(a1 is null)
            {
                if (a2 is null)
                    return true;
                return false;
            }
            return
                a1.Nazwa.ToLower() == a2.Name.ToLower() &&
                SafeToLower(a1.Opis) == SafeToLower(a2.Description) &&
                SafeToLower(a1.NazwaRozszerzona) == SafeToLower(a2.ExtendedName);
        }

        public static bool operator !=(ObszarZestawuPytan a1, AreaViewModel a2) =>
            !(a1 == a1);
    }
}