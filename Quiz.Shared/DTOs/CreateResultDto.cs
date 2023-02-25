using System;
using Quiz.Shared.ViewModels;
using System.ComponentModel.DataAnnotations;
using Quiz.Data.Models;

namespace Quiz.Shared.DTOs
{
	public class CreateResultDto
	{
        public long Id { get; set; }
        public int DiagnosisId { get; set; }
        public int RatingId { get; set; }
        [Range(1, 6)]
        public byte RatingLevel { get; set; }
        [MaxLength(2048)]
        public string? Notes { get; set; }

        public static explicit operator Wynik(CreateResultDto createResult) =>
            new Wynik
            {
                DiagnozaId = createResult.DiagnosisId,
                OcenaZestawuPytanId = createResult.RatingId,
                PoziomOceny = createResult.RatingLevel,
                Notatki = createResult.Notes,
                CzyAktywny = true
            };
    }
}

