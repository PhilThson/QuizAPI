using System.ComponentModel;

namespace Quiz.Data.Enums
{
    public enum StanowiskoEnum
    {
        [Description("Obsługa")]
        Obsluga,
        [Description("Konserwator")]
        Konserwator,
        [Description("Pomoc")]
        Pomoc,
        [Description("Pedagog")]
        Pedagog,
        [Description("Kierownik")]
        Kierownik,
        [Description("Dyrektor")]
        Dyrektor
    }
}
