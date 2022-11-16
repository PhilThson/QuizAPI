using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Data.Models;

namespace Quiz.Data.EntityConfiguration
{
    public class PracownicyAdresyConfig : IEntityTypeConfiguration<PracownicyAdresy>
    {
        public void Configure(EntityTypeBuilder<PracownicyAdresy> builder)
        {
            builder.HasKey(pa => new { pa.PracownikId, pa.AdresId });
        }
    }
}
