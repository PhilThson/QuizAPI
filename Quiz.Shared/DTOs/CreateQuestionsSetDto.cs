using System;
using Quiz.Shared.ViewModels;

namespace Quiz.Shared.DTOs
{
	public class CreateQuestionsSetDto
	{
        public string SkillDescription { get; set; }
        public byte AreaId { get; set; }
        public byte DifficultyId { get; set; }
        public IEnumerable<string> QuestionsSetRatings { get; set; }
        public IEnumerable<QuestionViewModel>? Questions { get; set; }
        public IEnumerable<AttachmentFileViewModel>? AttachmentFiles { get; set; }
    }
}

