using Quiz.Data.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Quiz.Data.Models
{
    public class ZestawPytan : BaseEntity<int>
    {
        public ZestawPytan()
        {
            ZestawPytanKartyPracy = new HashSet<KartaPracy>();
            ZestawPytanPytania = new HashSet<Pytanie>();
            ZestawPytanOceny = new HashSet<OcenaZestawuPytan>();
        }

        [MaxLength(2048)]
        public string OpisUmiejetnosci { get; set; }

        [JsonIgnore]
        public byte ObszarZestawuPytanId { get; set; }

        [ForeignKey(nameof(ObszarZestawuPytanId))]
        [InverseProperty("ObszarZestawuPytanZestawyPytan")]
        public virtual ObszarZestawuPytan ObszarZestawuPytan { get; set; }

        [JsonIgnore]
        public byte SkalaTrudnosciId { get; set; }

        [ForeignKey(nameof(SkalaTrudnosciId))]
        [InverseProperty("SkalaTrudnosciZestawyPytan")]
        public virtual SkalaTrudnosci SkalaTrudnosci { get; set; }

        public virtual ICollection<KartaPracy> ZestawPytanKartyPracy { get; set; }

        public virtual ICollection<Pytanie> ZestawPytanPytania { get; set; }

        public virtual ICollection<OcenaZestawuPytan> ZestawPytanOceny
        { get; set; }
    }
}
