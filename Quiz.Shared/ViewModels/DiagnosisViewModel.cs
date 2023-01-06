using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.Shared.ViewModels
{
	public class DiagnosisViewModel
	{
        public int Id { get; set; }
        [Required]
        [MaxLength(9)]
        public string SchoolYear { get; set; }
        public StudentViewModel Student { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public IList<ResultViewModel>? Results { get; set; }
        public DifficultyViewModel Difficulty { get; set; }
    }
}