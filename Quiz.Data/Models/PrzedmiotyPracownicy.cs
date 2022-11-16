using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.Data.Models
{
    public class PrzedmiotyPracownicy
    {
        public byte PrzedmiotId { get; set; }

        [ForeignKey(nameof(PrzedmiotId))]
        [InverseProperty("PrzedmiotPrzedmiotyPracownicy")]
        public Przedmiot Przedmiot { get; set; }

        public int PracownikId { get; set; }

        [ForeignKey(nameof(PracownikId))]
        [InverseProperty("PracownikPrzedmiotyPracownicy")]
        public Pracownik Pracownik { get; set; }
    }
}
