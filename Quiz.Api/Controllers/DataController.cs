using Microsoft.AspNetCore.Mvc;
using Quiz.Data.Helpers;
using Quiz.Infrastructure;
using Quiz.Infrastructure.Interfaces;
using Quiz.Shared.ViewModels;

namespace Quiz.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IDataService _dataService;

        public DataController(IDataService dataService)
        {
            _dataService = dataService;
        }

        #region Pracownicy
        [HttpGet("pracownicy")]
        public async Task<IEnumerable<EmployeeViewModel>> GetAllEmployees()
        {
            var employees = await _dataService.GetAllEmployees();

            return employees;
        }
        #endregion

        #region Uczniowie
        [HttpGet("uczniowie")]
        public async Task<IEnumerable<StudentViewModel>> GetAllStudents()
        {
            var students = await _dataService.GetAllStudents();

            return students;
        }
        #endregion

        #region Pytania
        [HttpGet("pytania")]
        public async Task<IEnumerable<QuestionViewModel>> GetAllQuestions()
        {
            var questions = await _dataService.GetAllQuestions();

            return questions;
        }

        [HttpGet("pytania/{id}", Name = nameof(GetQuestionById))]
        public async Task<IActionResult> GetQuestionById([FromRoute] int id)
        {
            try
            {
                var question = await _dataService.GetQuestionById(id);
                return Ok(question);
            }
            catch(DataNotFoundException e)
            {
                return NotFound();
            }
        }

        [HttpPost("pytania")]
        public async Task<IActionResult> CreateQuestion(QuestionViewModel questionVM)
        {
            try
            {
                if (questionVM.Id.HasValue)
                    return BadRequest("Przesłany obiekt nie może posiadać identyfikatora");

                var question = await _dataService.AddQuestion(questionVM);

                return CreatedAtRoute(nameof(GetQuestionById),
                    new { id = question.Id}, question);
            }
            catch (DataValidationException e) { return BadRequest(e.Message); }
            catch (Exception e) { return BadRequest(); }
        }

        [HttpPut("pytania")]
        public async Task<IActionResult> UpdateQuestion(QuestionViewModel questionVM)
        {
            try
            {
                var question = await _dataService.UpdateQuestion(questionVM);
                return Ok(question);
            }
            catch (DataValidationException e) { return BadRequest(e.Message); }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(); }
        }
        #endregion

        #region Zestawy pytań
        [HttpGet("zestawyPytan")]
        public async Task<IEnumerable<QuestionsSetViewModel>> GetAllQuestionsSets()
        {
            var questionsSets = await _dataService.GetAllQuestionsSets();
            return questionsSets;
        }

        [HttpGet("zestawyPytan/{id}", Name = nameof(GetQuestionsSetById))]
        public async Task<IActionResult> GetQuestionsSetById([FromRoute] int id)
        {
            try
            {
                var questionsSet = await _dataService.GetQuestionsSetById(id);
                return Ok(questionsSet);
            }
            catch (DataNotFoundException e)
            {
                return NotFound();
            }
        }

        [HttpPatch("zestawyPytan/{id}/skill")]
        public async Task<IActionResult> UpdateSkill([FromRoute] int id,
            [FromBody] string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length > 2048)
                    return BadRequest("Przesłano nieprawidłowy model");

                var updated = await _dataService.UpdateSkillDescription(id, value);
                return Ok(updated);
            }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(); }
        }
        #endregion

        #region Obszary zestawu pytań
        [HttpGet("obszary")]
        public async Task<IEnumerable<AreaViewModel>> GetAllAreas()
        {
            var areas = await _dataService.GetAllAreas();
            return areas;
        }

        [HttpPut("obszary")]
        public async Task<IActionResult> UpdateArea([FromBody] AreaViewModel areaVM)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                var updated = await _dataService.UpdateArea(areaVM);
                return Ok(updated);
            }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(); }
        }
        #endregion

        #region Skale trudności
        [HttpGet("skaleTrudnosci")]
        public async Task<IEnumerable<DifficultyViewModel>> GetAllDifficulties()
        {
            var difficulties = await _dataService.GetAllDifficulties();
            return difficulties;
        }

        [HttpPut("skaleTrudnosci")]
        public async Task<IActionResult> UpdateDifficulty([FromBody]
            DifficultyViewModel difficultyVM)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                var updated = await _dataService.UpdateDifficulty(difficultyVM);
                return Ok(updated);
            }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(); }
        }
        #endregion

        #region Karty pracy
        [HttpGet("kartyPracy/{id}", Name = nameof(GetAttachmentById))]
        public async Task<IActionResult> GetAttachmentById([FromRoute] int id)
        {
            try
            {
                var attachment = await _dataService.GetAttachmentById(id);
                return Ok(attachment);
            }
            catch (DataNotFoundException e)
            {
                return NotFound();
            }
        }
        #endregion
    }
}
