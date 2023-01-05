using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Quiz.Data.Models
{
    public class Uczen : Osoba
    {
        public Uczen()
        {
            UczenOceny = new HashSet<Ocena>();
            UczenDiagnozy = new HashSet<Diagnoza>();
        }
        
        public byte OddzialId { get; set; }

        [Required]
        [ForeignKey(nameof(OddzialId))]
        [InverseProperty("OddzialUczniowie")]
        public virtual Oddzial Oddzial { get; set; }

        [Required]
        [StringLength(15)]
        public string NrOrzeczenia { get; set; }

        //public int? AdresId { get; set; }

        //[ForeignKey(nameof(AdresId))]
        //[InverseProperty("AdresUczniowie")]
        //public virtual Adres Adres { get; set; }

        public virtual ICollection<Ocena> UczenOceny { get; set; }

        public virtual ICollection<Diagnoza> UczenDiagnozy { get; set; }
    }
}
