using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Data.Models;

namespace Quiz.Data.EntityConfiguration
{
	public class SkalaTrudnosciConfig : IEntityTypeConfiguration<SkalaTrudnosci>
	{
        public void Configure(EntityTypeBuilder<SkalaTrudnosci> builder)
        {
            builder.HasQueryFilter(u => u.CzyAktywny);
        }
    }
}

