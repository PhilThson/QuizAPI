using System.ComponentModel;

namespace Quiz.Data.Enums
{
    public enum ObszarZestawuPytanEnum
    {
        [Description("Myślenie")]
        PierwszyObszar,
        [Description("Lateralizacja")]
        DrugiObszar,
        [Description("Strefa słowna")]
        TrzeciObszar,
        [Description("Strefa bezsłowna")]
        CzwartyObszar,
        [Description("Podstawowe wiadomości")]
        PiatyObszar,
        [Description("Percepcja słuchowa")]
        SzostyObszar
    }
}
