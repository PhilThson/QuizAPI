using Quiz.Data.Models.Base;

namespace Quiz.Data.Models
{
    public class Etat : BaseDictionaryEntity<byte>
    {
        public Etat()
        {
            EtatPracownicy = new HashSet<Pracownik>();
        }

        public virtual ICollection<Pracownik> EtatPracownicy { get; set; }
    }
}
