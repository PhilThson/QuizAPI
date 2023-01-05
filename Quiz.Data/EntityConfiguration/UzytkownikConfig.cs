using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Data.Models;

namespace Quiz.Data.EntityConfiguration
{
    public class UzytkownikConfig : IEntityTypeConfiguration<Uzytkownik>
    {
        public void Configure(EntityTypeBuilder<Uzytkownik> builder)
        {
            builder
                .HasIndex(u => u.Email)
                .IsUnique(true);
        }
    }
}
