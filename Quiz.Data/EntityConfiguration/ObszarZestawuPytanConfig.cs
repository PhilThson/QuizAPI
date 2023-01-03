using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Data.Models;

namespace Quiz.Data.EntityConfiguration
{
    public class ObszarZestawuPytanConfig : IEntityTypeConfiguration<ObszarZestawuPytan>
    {
        public void Configure(EntityTypeBuilder<ObszarZestawuPytan> builder)
        {
            builder.HasQueryFilter(u => u.CzyAktywny);
        }
    }
}

