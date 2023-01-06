using System.ComponentModel;

namespace Quiz.Data.Enums
{
    public enum SkalaTrudnosciEnum
    {
        [Description("Łatwy")]
        A,
        [Description("Średni")]
        B,
        [Description("Trudny")]
        C,
        [Description("Łatwy i średni")]
        AB,
        [Description("Średni i trudny")]
        BC,
        [Description("Łatwy, średni i trudny")]
        ABC
    }
}
