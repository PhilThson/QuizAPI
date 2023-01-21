using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Quiz.Data.Models;

namespace Quiz.Shared.ViewModels
{
    public class QuestionViewModel
	{
		public int Id { get; set; }
        [Required]
        [MaxLength(2048)]
        [DisplayName("Treść")]
        public string Content { get; set; }
        [Required]
        [MaxLength(2048)]
        [DisplayName("Opis")]
        public string Description { get; set; }
        [DisplayName("Id zestawu pytań")]
        public int QuestionsSetId { get; set; }

        public static bool operator ==(Pytanie p, QuestionViewModel q)
        {
            if (p is null)
            {
                if (q is null)
                    return true;
                return false;
            }
            return
                p.Tresc.ToLower() == q.Content.ToLower() &&
                p.Opis.ToLower() == q.Description.ToLower() &&
                p.ZestawPytanId == q.QuestionsSetId;
        }

        public static bool operator !=(Pytanie p, QuestionViewModel q) =>
            !(p == q);
	}
}