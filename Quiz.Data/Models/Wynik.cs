using Quiz.Data.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Quiz.Data.Models
{
    public class Wynik : BaseEntity<long>
    {
        [JsonIgnore]
        public int OcenaZestawuPytanId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(OcenaZestawuPytanId))]
        [InverseProperty("OcenaZestawuPytanWyniki")]
        public virtual OcenaZestawuPytan OcenaZestawuPytan { get; set; }

        [Range(1, 6)]
        public byte PoziomOceny { get; set; }

        [JsonIgnore]
        public int DiagnozaId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(DiagnozaId))]
        [InverseProperty("DiagnozaWyniki")]
        public virtual Diagnoza Diagnoza { get; set; }

        [MaxLength(2048)]
        public string Notatki { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DataCzasWpisu { get; set; }
    }
}
