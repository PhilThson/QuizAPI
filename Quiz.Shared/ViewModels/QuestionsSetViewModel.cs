﻿using System;
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
        public string Area { get; set; }
        [DisplayName("Skala trudności")]
        public string Difficulty { get; set; }
        [DisplayName("Oceny zestawu pytań")]
        public string[] QuestionsSetRatings { get; set; }
        [DisplayName("Pytania")]
        public List<QuestionViewModel> Questions { get; set; }
        [DisplayName("Dołączony plik - Karta pracy")]
        public AttachmentFileViewModel AttachmentFile { get; set; }
    }
}