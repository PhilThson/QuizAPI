using System.ComponentModel.DataAnnotations;

namespace Quiz.Shared.ViewModels
{
    public class DiagnosisViewModel
	{
        public int Id { get; set; }
        public string SchoolYear { get; set; }
        public StudentViewModel Student { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public IList<ResultViewModel>? Results { get; set; }
        public DifficultyViewModel Difficulty { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ReportId { get; set; }
        public string Institution { get; set; }
    }
}