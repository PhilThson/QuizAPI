using Quiz.Data.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Quiz.Data.Models
{
    public class OcenaZestawuPytan : BaseEntity<int>
    {
        public OcenaZestawuPytan()
        {
            OcenaZestawuPytanWyniki = new HashSet<Wynik>();
        }

        [MaxLength(1024)]
        public string OpisOceny { get; set; }

        [JsonIgnore]
        public int ZestawPytanId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(ZestawPytanId))]
        [InverseProperty("ZestawPytanOceny")]
        public virtual ZestawPytan ZestawPytan { get; set; }

        [JsonIgnore]
        public virtual ICollection<Wynik> OcenaZestawuPytanWyniki { get; set; }
    }
}
