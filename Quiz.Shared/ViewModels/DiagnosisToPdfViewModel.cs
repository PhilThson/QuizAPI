﻿using System;
using System.ComponentModel;

namespace Quiz.Shared.ViewModels
{
	public class DiagnosisToPdfViewModel
	{
        public int Id { get; set; }
        [DisplayName("Rok szkolny:")]
        public string SchoolYear { get; set; }
        [DisplayName("Uczeń:")]
        public StudentViewModel Student { get; set; }
        [DisplayName("Prowadzący:")]
        public EmployeeViewModel Employee { get; set; }
        [DisplayName("Skala trudności diagnozy:")]
        public DifficultyViewModel Difficulty { get; set; }
        [DisplayName("Data utworzenia diagnozy:")]
        public DateTime CreatedDate { get; set; }
        [DisplayName("Wyniki przeprowadzonej diagnozy")]
        public IList<ResultViewModel>? Results { get; set; }
        [DisplayName("Zestawy pytań - opanowane")]
        public IList<QuestionsSetViewModel> QuestionsSetsMastered { get; set; }
        [DisplayName("Zestawy pytań - do poprawy")]
        public IList<QuestionsSetViewModel> QuestionsSetsToImprove { get; set; }
    }
}

