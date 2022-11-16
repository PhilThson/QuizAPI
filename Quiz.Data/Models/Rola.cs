using Quiz.Data.Models.Base;

namespace Quiz.Data.Models
{
    public class Rola : BaseDictionaryEntity<byte>
    {
        public Rola()
        {
            RolaUzytkownicy = new HashSet<Uzytkownik>();
        }

        public virtual ICollection<Uzytkownik> RolaUzytkownicy { get; set; }
    }
}
