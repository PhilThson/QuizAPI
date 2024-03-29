﻿using System.Linq.Expressions;
using Quiz.Data.Models;
using Quiz.Shared.DTOs;
using Quiz.Shared.DTOs.Read;
using Quiz.Shared.ViewModels;

namespace Quiz.Infrastructure.Interfaces
{
    public interface IDataService
    {
        Task<IEnumerable<EmployeeViewModel>> GetAllEmployees();
        Task<EmployeeViewModel> GetEmployeeById(int id);
        Task<EmployeeViewModel> AddEmployee(CreateEmployeeDto employeeDto);
        Task<EmployeeViewModel> UpdateEmployee(CreateEmployeeDto employeeDto);
        Task DeleteEmployeeById(int id);

        Task<IEnumerable<StudentViewModel>> GetAllStudents();
        Task<StudentViewModel> GetStudentById(int id);
        Task<StudentViewModel> AddStudent(CreateStudentDto studentDto);
        Task<StudentViewModel> UpdateStudent(CreateStudentDto studentDto);
        Task DeleteStudentById(int id);

        Task<IEnumerable<AddressDto>> GetAllAddresses();
        Task<AddressDto> GetAddressById(int id);
        Task<AddressDto> AddAddress(AddressDto addressDto);
        Task<AddressDto> UpdateAddress(AddressDto addressDto);
        Task DeleteAddressById(int id);

        //Task<IEnumerable<QuestionsSetViewModel>> GetAllQuestionsSets();
        Task<IEnumerable<QuestionsSetViewModel>> GetQuestionsSetsByCondition(
                Expression<Func<ZestawPytan, bool>>? filter = null);
        Task<QuestionsSetViewModel> GetQuestionsSetById(int id);
        Task<string> UpdateSkillDescription(int id, string value);
        Task<AreaViewModel> UpdateQuestionsSetArea(int id, byte areaId);
        Task<DifficultyViewModel> UpdateQuestionsSetDifficulty(int id,
            byte difficultyId);
        Task<QuestionsSetViewModel> AddQuestionsSet(
            CreateQuestionsSetDto createQuestionsSet);
        Task DeleteQuestionsSetById(int id);

        Task<IEnumerable<QuestionViewModel>> GetAllQuestions();
        Task<QuestionViewModel> GetQuestionById(int id);
        Task<QuestionViewModel> AddQuestion(QuestionViewModel questionVM);
        Task<QuestionViewModel> UpdateQuestion(QuestionViewModel questionVM);
        Task DeleteQuestionById(int id);

        Task<IEnumerable<AreaViewModel>> GetAllAreas();
        Task<AreaViewModel> GetAreaById(byte id);
        Task<AreaViewModel> UpdateArea(AreaViewModel areaVM);
        Task DeleteAreaById(byte id);
        Task<AreaViewModel> AddArea(CreateDictionaryDto areaDto);

        Task<IEnumerable<DifficultyViewModel>> GetAllDifficulties();
        Task<DifficultyViewModel> GetDifficultyById(byte id);
        Task<DifficultyViewModel> UpdateDifficulty(
            DifficultyViewModel difficultyVM);
        Task DeleteDifficultyById(byte id);
        Task<DifficultyViewModel> AddDifficulty(CreateDictionaryDto difficultyDto);

        Task<RatingViewModel> GetRatingById(int id);
        Task<RatingViewModel> UpdateRating(RatingViewModel ratingVM);
        Task<List<RatingViewModel>> GetRatingsByQuestionsSetId(
            int questionsSetId);

        Task<AttachmentFileViewModel> GetAttachmentById(int id);

        Task<IEnumerable<DiagnosisViewModel>> GetAllDiagnosis();
        Task<DiagnosisViewModel> GetDiagnosisById(int id);
        Task<DiagnosisViewModel> AddDiagnosis(
            CreateDiagnosisDto createDiagnosis);
        Task<BaseReportDto> AddDiagnosisReport(DiagnosisViewModel diagnosis);

        Task<ResultViewModel> GetResultById(long id);
        Task<ResultViewModel> AddDiagnosisResult(CreateResultDto createResult);
        Task<ResultViewModel> GetResultByDiagnosisQuestionsSetIds(
            int diagnosisId, int questionsSetId);

        Task<IEnumerable<JobDto>> GetAllJobs();
        Task<IEnumerable<PositionDto>> GetAllPositions();
        Task<IEnumerable<BranchDto>> GetAllBranches();
        Task<IEnumerable<RoleDto>> GetAllRoles();

        Task<UserDto> GetUserById(int userId);
        Task<UserDto> GetUserByEmail(string email);
        Task<UserDto> AddUser(CreateUserDto userDto);

        Task<ReportDto> GetReportById(int reportId);
    }
}
