using System;
using System.ComponentModel.DataAnnotations;
using Quiz.Shared.DTOs;

namespace Quiz.Shared.ViewModels
{
	public class ResultViewModel
	{
		public long Id { get; set; }
		public RatingDto? QuestionsSetRating { get; set; }
		[Range(1, 6)]
		public byte RatingLevel { get; set; }
		[MaxLength(2048)]
		public string? Notes { get; set; }
        public int DiagnosisId { get; set; }
    }
}