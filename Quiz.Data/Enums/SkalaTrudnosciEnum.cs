using System.ComponentModel;

namespace Quiz.Data.Enums
{
    public enum SkalaTrudnosciEnum
    {
        [Description("Pierwsza skala")]
        A,
        [Description("Druga skala")]
        B,
        [Description("Trzecia skala")]
        C,
        [Description("Czwarta skala")]
        AB,
        [Description("Piąta skala")]
        AC,
        [Description("Szósta skala")]
        BC,
        [Description("Siódma skala")]
        ABC
    }
}
