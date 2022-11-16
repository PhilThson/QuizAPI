using System.ComponentModel;

namespace Quiz.Data.Enums
{
    public enum EtatEnum
    {
        [Description("Pracownik Administracyjny")]
        PracownikAdministracyjny,
        [Description("Pracownik Pedagogiczny")]
        PracownikPedagogiczny,
        [Description("Pracownik Obsługi")]
        PracownikObslugi
    }
}
