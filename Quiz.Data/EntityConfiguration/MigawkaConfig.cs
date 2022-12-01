using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Data.Models;

namespace Quiz.Data.EntityConfiguration
{
	public class MigawkaConfig : IEntityTypeConfiguration<Migawka>
	{
        public void Configure(EntityTypeBuilder<Migawka> builder)
        {
            builder
                .Property(m => m.DataZmiany)
                .HasColumnType("datetime")
                .HasComputedColumnSql("getdate()");
        }
    }
}

