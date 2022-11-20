using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Data.Models;

namespace Quiz.Data.EntityConfiguration
{
	public class KartaPracyConfig : IEntityTypeConfiguration<KartaPracy>
	{
        public void Configure(EntityTypeBuilder<KartaPracy> builder)
        {
            builder
                .HasOne(kp => kp.ZestawPytan)
                .WithMany(zp => zp.ZestawPytanKartyPracy)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

