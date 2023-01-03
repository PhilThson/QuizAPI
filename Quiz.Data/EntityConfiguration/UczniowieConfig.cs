using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Data.Models;

namespace Quiz.Data.EntityConfiguration
{
    public class UczniowieConfig : IEntityTypeConfiguration<Uczen>
    {
        public void Configure(EntityTypeBuilder<Uczen> builder)
        {
            builder
                .HasOne(u => u.Oddzial)
                .WithMany(o => o.OddzialUczniowie)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasMany(u => u.UczenOceny)
                .WithOne(o => o.Uczen)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasQueryFilter(u => u.CzyAktywny);
        }
    }
}
