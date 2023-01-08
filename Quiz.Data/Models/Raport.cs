using Quiz.Data.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Quiz.Data.Models
{
    public class Raport : BaseDictionaryEntity<int>
    {
        public byte[] Zawartosc { get; set; }

        public long Rozmiar { get; set; }

        public int DiagnozaId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(DiagnozaId))]
        [InverseProperty("DiagnozaRaport")]
        public virtual Diagnoza Diagnoza { get; set; }
    }
}
