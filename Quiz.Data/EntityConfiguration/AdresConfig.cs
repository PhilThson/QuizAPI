using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Data.Models;

namespace Quiz.Data.EntityConfiguration
{
    public class AdresConfig : IEntityTypeConfiguration<Adres>
    {
        public void Configure(EntityTypeBuilder<Adres> builder)
        {
            builder.HasQueryFilter(a => a.CzyAktywny);
        }
    }
}

