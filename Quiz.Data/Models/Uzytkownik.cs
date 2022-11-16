﻿using Quiz.Data.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.Data.Models
{
    /// <summary>
    /// Klasa opisująca użytkownika korzystającego z aplikacji
    /// </summary>
    public class Uzytkownik : BaseEntity<int>
    {
        public Uzytkownik()
        {
            UzytkownikMigawki = new HashSet<Migawka>();
        }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Imie { get; set; }

        [MaxLength(50)]
        public string Nazwisko { get; set; }

        [MaxLength(200)]
        public string HashHasla { get; set; }

        public byte RolaId { get; set; }

        [ForeignKey(nameof(RolaId))]
        [InverseProperty("RolaUzytkownicy")]
        public virtual Rola Rola { get; set; }

        public virtual ICollection<Migawka> UzytkownikMigawki { get; set; }
    }
}
