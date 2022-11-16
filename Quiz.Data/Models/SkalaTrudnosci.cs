using Quiz.Data.Models.Base;

namespace Quiz.Data.Models
{
    public class SkalaTrudnosci : BaseDictionaryEntity<byte>
    {
        public SkalaTrudnosci()
        {
            SkalaTrudnosciZestawyPytan = new HashSet<ZestawPytan>();
        }

        public virtual ICollection<ZestawPytan> SkalaTrudnosciZestawyPytan
        { get; set; }
    }
}
