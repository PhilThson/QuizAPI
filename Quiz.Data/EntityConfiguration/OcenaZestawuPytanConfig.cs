using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Data.Models;

namespace Quiz.Data.EntityConfiguration
{
    public class OcenaZestawuPytanConfig : IEntityTypeConfiguration<OcenaZestawuPytan>
    {
        public void Configure(EntityTypeBuilder<OcenaZestawuPytan> builder)
        {
            builder
                .HasOne(o => o.ZestawPytan)
                .WithMany(p => p.ZestawPytanOceny)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
