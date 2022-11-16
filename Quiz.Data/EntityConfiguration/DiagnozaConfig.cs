﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Data.Models;

namespace Quiz.Data.EntityConfiguration
{
	public class DiagnozaConfig : IEntityTypeConfiguration<Diagnoza>
	{
        public void Configure(EntityTypeBuilder<Diagnoza> builder)
        {
            builder
                .HasOne(d => d.Pracownik)
                .WithMany(p => p.PracownikDiagnozy)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(d => d.Uczen)
                .WithMany(u => u.UczenDiagnozy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

