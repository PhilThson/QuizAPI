﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Quiz.Data.Models.Base;

namespace Quiz.Data.Models
{
    public abstract class Osoba : BaseEntity<int>
    {
        [Required]
        [MaxLength(20)]
        public string Imie { get; set; }

        [MaxLength(20)]
        public string? DrugieImie { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nazwisko { get; set; }

        [MaxLength(10)]
        [Column(TypeName = "date")]
        public DateTime? DataUrodzenia { get; set; }

        [MaxLength(256)]
        public string? MiejsceUrodzenia { get; set; }

        [Required]
        [MaxLength(11)]
        [Column(TypeName = "varchar(11)")]
        public string Pesel { get; set; }

        public static bool operator ==(Osoba o1, Osoba o2)
        {
            return o1.GetHashCode() == o2.GetHashCode();
        }
        public static bool operator !=(Osoba o1, Osoba o2)
        {
            return !(o1 == o2);
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Osoba)) return false;
            Osoba o = (Osoba)obj;
            return (this == o);
        }
        public override int GetHashCode()
        {
            return Imie.GetHashCode() ^ Nazwisko.GetHashCode() ^ Pesel.GetHashCode();
        }
    }
}
