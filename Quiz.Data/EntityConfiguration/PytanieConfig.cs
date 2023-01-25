using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Data.Models;

namespace Quiz.Data.EntityConfiguration
{
    public class PytanieConfig : IEntityTypeConfiguration<Pytanie>
    {
        public void Configure(EntityTypeBuilder<Pytanie> builder)
        {
            builder
                .HasOne(p => p.ZestawPytan)
                .WithMany(z => z.ZestawPytanPytania)
                .OnDelete(DeleteBehavior.NoAction);

            //builder.HasQueryFilter(p => p.CzyAktywny);
        }
    }
}
