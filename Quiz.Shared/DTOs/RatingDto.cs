using System;
namespace Quiz.Shared.DTOs
{
	public class RatingDto
	{
        public int Id { get; set; }
        public string? RatingDescription { get; set; }
        public int QuestionsSetId { get; set; }
    }
}

