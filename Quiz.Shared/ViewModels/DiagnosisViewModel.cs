using System.ComponentModel.DataAnnotations;

namespace Quiz.Shared.ViewModels
{
    public class DiagnosisViewModel
	{
        public int Id { get; set; }
        public string Institution { get; set; }
        public string SchoolYear { get; set; }
        public string CounselingCenter { get; set; }
        public StudentViewModel Student { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public IList<ResultViewModel>? Results { get; set; }
        public DifficultyViewModel Difficulty { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ReportId { get; set; }

        public static explicit operator DiagnosisToPdfViewModel(DiagnosisViewModel diagnosis) =>
            new DiagnosisToPdfViewModel
            {
                Id = diagnosis.Id,
                Institution = diagnosis.Institution,
                SchoolYear = diagnosis.SchoolYear,
                CounselingCenter = diagnosis.CounselingCenter,
                Student = diagnosis.Student,
                Employee = diagnosis.Employee,
                CreatedDate = diagnosis.CreatedDate,
                Difficulty = diagnosis.Difficulty,
                Results = diagnosis.Results,
                QuestionsSetsMastered = new List<QuestionsSetViewModel>(),
                QuestionsSetsToImprove = new List<QuestionsSetViewModel>()
            };
    }
}