using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Quiz.Shared.ViewModels
{
	public class CreateQuestionsSetViewModel
	{
        [Required]
        [MaxLength(2048)]
        public string SkillDescription { get; set; }
        public byte AreaId { get; set; }
        public byte DifficultyId { get; set; }
        public IEnumerable<RatingViewModel> QuestionsSetRatings { get; set; }
        [DisplayName("Pytania")]
        public IEnumerable<QuestionViewModel>? Questions { get; set; }
        [DisplayName("Dołączone pliki - Karty pracy")]
        public IEnumerable<AttachmentFileViewModel> AttachmentFiles { get; set; }
    }
}

