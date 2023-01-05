using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Quiz.Data.Models.Base;

namespace Quiz.Data.Models
{
    public class Diagnoza : BaseEntity<int>
	{
		public Diagnoza()
		{
            DiagnozaWyniki = new HashSet<Wynik>();
		}

		[Required]
		[MaxLength(9)]
		public string RokSzkolny { get; set; }

        public int UczenId { get; set; }

        [ForeignKey(nameof(UczenId))]
        [InverseProperty("UczenDiagnozy")]
        public virtual Uczen Uczen { get; set; }

        public int PracownikId { get; set; }

        [ForeignKey(nameof(PracownikId))]
        [InverseProperty("PracownikDiagnozy")]
        public virtual Pracownik Pracownik { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DataPrzeprowadzenia { get; set; }

        public byte SkalaTrudnosciId { get; set; }

        [ForeignKey(nameof(SkalaTrudnosciId))]
        [InverseProperty("SkalaTrudnosciDiagnozy")]
        public virtual SkalaTrudnosci DiagnozaSkalaTrudnosci { get; set; }

        //Składany z opisów?
        //public string RozwojEmocjonalnoSpoleczny { get; set; }

        [JsonIgnore]
        public virtual ICollection<Wynik> DiagnozaWyniki { get; set; }
    }
}

