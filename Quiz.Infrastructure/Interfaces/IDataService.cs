using Quiz.Data.Models;
using Quiz.Shared.ViewModels;

namespace Quiz.Infrastructure.Interfaces
{
    public interface IDataService
    {
        Task<IEnumerable<EmployeeViewModel>> GetAllEmployees();
        Task<IEnumerable<StudentViewModel>> GetAllStudents();
        Task<IEnumerable<QuestionsSetViewModel>> GetAllQuestionsSets();
        Task<QuestionsSetViewModel> GetQuestionsSetById(int id);
        Task<IEnumerable<QuestionViewModel>> GetAllQuestions();
        Task<QuestionViewModel> GetQuestionById(int id);
        Task<AttachmentFileViewModel> GetAttachmentById(int id);
        Task<QuestionViewModel> AddQuestion(QuestionViewModel questionVM);
        Task<QuestionViewModel> UpdateQuestion(QuestionViewModel questionVM);
        Task<ZestawPytan> AddQuestionsSet(
            QuestionsSetViewModel questionsSetVM);
        Task<string> UpdateSkillDescription(int id, string value);
        Task<IEnumerable<AreaViewModel>> GetAllAreas();
        Task<AreaViewModel> GetAreaById(byte id);
        Task<AreaViewModel> UpdateArea(AreaViewModel areaVM);
        Task<IEnumerable<DifficultyViewModel>> GetAllDifficulties();
        Task<DifficultyViewModel> GetDifficultyById(byte id);
        Task<DifficultyViewModel> UpdateDifficulty(
            DifficultyViewModel difficultyVM);
    }
}
