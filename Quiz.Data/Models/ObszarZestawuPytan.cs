using Quiz.Data.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Quiz.Data.Models
{
    public class ObszarZestawuPytan : BaseDictionaryEntity<byte>
    {
        public ObszarZestawuPytan()
        {
            ObszarZestawuPytanZestawyPytan = new HashSet<ZestawPytan>();
        }

        [MaxLength(1024)]
        public string? NazwaRozszerzona { get; set; }

        [JsonIgnore]
        public virtual ICollection<ZestawPytan> ObszarZestawuPytanZestawyPytan { get; set; }
    }
}
