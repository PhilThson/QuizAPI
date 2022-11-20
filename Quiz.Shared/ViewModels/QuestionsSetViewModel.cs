using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Quiz.Shared.ViewModels
{
	public class QuestionsSetViewModel
	{
		public int  Id { get; set; }
        [Required]
        [MaxLength(2048)]
        [DisplayName("Opis umiejętności")]
        public string SkillDescription { get; set; }
        [DisplayName("Obszar zestawu pytań")]
        public AreaViewModel Area { get; set; }
        [DisplayName("Skala trudności")]
        public DifficultyViewModel Difficulty { get; set; }
        [DisplayName("Oceny zestawu pytań")]
        public IEnumerable<RatingViewModel> QuestionsSetRatings { get; set; }
        [DisplayName("Pytania")]
        public IEnumerable<QuestionViewModel> Questions { get; set; }
        [DisplayName("Dołączone pliki - Karty pracy")]
        public IEnumerable<AttachmentViewModel> Attachments { get; set; }
    }
}