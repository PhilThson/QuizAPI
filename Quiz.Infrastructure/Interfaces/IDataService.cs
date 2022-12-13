using Quiz.Data.Models;
using Quiz.Shared.DTOs;
using Quiz.Shared.ViewModels;

namespace Quiz.Infrastructure.Interfaces
{
    public interface IDataService
    {
        Task<IEnumerable<EmployeeViewModel>> GetAllEmployees();
        Task<EmployeeViewModel> GetEmployeeById(int id);

        Task<IEnumerable<StudentViewModel>> GetAllStudents();

        Task<IEnumerable<QuestionsSetViewModel>> GetAllQuestionsSets();
        Task<QuestionsSetViewModel> GetQuestionsSetById(int id);
        Task<string> UpdateSkillDescription(int id, string value);
        Task<AreaViewModel> UpdateQuestionsSetArea(int id, byte areaId);
        Task<DifficultyViewModel> UpdateQuestionsSetDifficulty(int id,
            byte difficultyId);
        Task<QuestionsSetViewModel> AddQuestionsSet(
            CreateQuestionsSetDto createQuestionsSet);

        Task<IEnumerable<QuestionViewModel>> GetAllQuestions();
        Task<QuestionViewModel> GetQuestionById(int id);
        Task<QuestionViewModel> AddQuestion(QuestionViewModel questionVM);
        Task<QuestionViewModel> UpdateQuestion(QuestionViewModel questionVM);

        Task<IEnumerable<AreaViewModel>> GetAllAreas();
        Task<AreaViewModel> GetAreaById(byte id);
        Task<AreaViewModel> UpdateArea(AreaViewModel areaVM);

        Task<IEnumerable<DifficultyViewModel>> GetAllDifficulties();
        Task<DifficultyViewModel> GetDifficultyById(byte id);
        Task<DifficultyViewModel> UpdateDifficulty(
            DifficultyViewModel difficultyVM);

        Task<RatingViewModel> GetRatingById(int id);
        Task<RatingViewModel> UpdateRating(RatingViewModel ratingVM);
        Task<List<RatingViewModel>> GetRatingsByQuestionsSetId(
            int questionsSetId);

        Task<AttachmentFileViewModel> GetAttachmentById(int id);

        Task<IEnumerable<DiagnosisViewModel>> GetAllDiagnosis();
        Task<DiagnosisViewModel> GetDiagnosisById(int id);
        Task<DiagnosisViewModel> AddDiagnosis(
            CreateDiagnosisDto createDiagnosis);

        Task<ResultViewModel> GetResultById(long id);
        Task<ResultViewModel> AddDiagnosisResult(CreateResultDto createResult);
        Task<ResultViewModel> GetResultByDiagnosisQuestionsSetIds(
            int diagnosisId, int questionsSetId);
    }
}
