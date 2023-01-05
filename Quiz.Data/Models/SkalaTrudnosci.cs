using System.Text.Json.Serialization;
using Quiz.Data.Models.Base;

namespace Quiz.Data.Models
{
    public class SkalaTrudnosci : BaseDictionaryEntity<byte>
    {
        public SkalaTrudnosci()
        {
            SkalaTrudnosciZestawyPytan = new HashSet<ZestawPytan>();
            SkalaTrudnosciDiagnozy = new HashSet<Diagnoza>();
        }

        [JsonIgnore]
        public virtual ICollection<ZestawPytan> SkalaTrudnosciZestawyPytan
        { get; set; }

        [JsonIgnore]
        public virtual ICollection<Diagnoza> SkalaTrudnosciDiagnozy { get; set; }
    }
}
