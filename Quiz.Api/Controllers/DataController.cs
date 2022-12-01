using Microsoft.AspNetCore.Mvc;
using Quiz.Data.Helpers;
using Quiz.Infrastructure;
using Quiz.Infrastructure.Interfaces;
using Quiz.Shared.DTOs;
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
        public async Task<IActionResult> AddQuestion(QuestionViewModel questionVM)
        {
            try
            {
                if (questionVM.Id != default(int))
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
            catch (DataNotFoundException e) { return NotFound(); }
            catch (Exception e) { return BadRequest(); }
        }

        [HttpPost("zestawyPytan")]
        public async Task<IActionResult> AddQuestionsSet(
            CreateQuestionsSetDto createQuestionsSet)
        {
            try
            {
                var createdQS = await _dataService.AddQuestionsSet(createQuestionsSet);
                return CreatedAtRoute(nameof(GetQuestionsSetById),
                    new { id = createdQS.Id }, createdQS);
            }
            catch (DataValidationException e) { return BadRequest(e.Message); }
            catch (DataNotFoundException e) { return NotFound(); }
            catch (Exception e) { return BadRequest(e.Message); }
        }

        [HttpPatch("zestawyPytan/{id}/skill")]
        public async Task<IActionResult> UpdateSkill([FromRoute] int id,
            [FromQuery] string skill)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(skill) || skill.Length > 2048)
                    return BadRequest("Przesłano nieprawidłowy model");

                var updated = await _dataService.UpdateSkillDescription(id, skill);
                return Ok(updated);
            }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(); }
        }

        [HttpPatch("zestawyPytan/{id}/area")]
        public async Task<IActionResult> UpdateQuestionsSetArea([FromRoute] int id,
            [FromQuery] byte areaId)
        {
            try
            {
                if (areaId == default(byte))
                    return BadRequest("Przesłano nieprawidłowy model");

                var updated = await _dataService.UpdateQuestionsSetArea(id, areaId);
                return Ok(updated);
            }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(); }
        }

        [HttpPatch("zestawyPytan/{id}/difficulty")]
        public async Task<IActionResult> UpdateQuestionsSetDifficulty(
            [FromRoute] int id, [FromQuery] byte difficultyId)
        {
            try
            {
                if (difficultyId == default(byte))
                    return BadRequest("Przesłano nieprawidłowy model");

                var updated = await _dataService.UpdateQuestionsSetDifficulty(id,
                    difficultyId);
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

        [HttpGet("obszary/{id}")]
        public async Task<IActionResult> GetAreaById(byte id)
        {
            try
            {
                var area = await _dataService.GetAreaById(id);
                return Ok(area);
            }
            catch (DataNotFoundException e) { return NotFound(); }
            catch (Exception e) { return BadRequest(); }
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

        [HttpGet("skaleTrudnosci/{id}")]
        public async Task<IActionResult> GetDifficultyById(byte id)
        {
            try
            {
                var difficulty = await _dataService.GetDifficultyById(id);
                return Ok(difficulty);
            }
            catch (DataNotFoundException e) { return NotFound(); }
            catch (Exception e) { return BadRequest(); }
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

        #region Oceny zestawu pytań
        [HttpGet("ocenyZestawuPytan/{id}")]
        public async Task<IActionResult> GetRatingById([FromRoute] int id)
        {
            try
            {
                var rating = await _dataService.GetRatingById(id);
                return Ok(rating);
            }
            catch (DataNotFoundException e) { return NotFound(); }
            catch (Exception e) { return BadRequest(); }
        }

        [HttpPut("ocenyZestawuPytan")]
        public async Task<IActionResult> UpdateRating([FromBody] RatingViewModel ratingVM)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                var updated = await _dataService.UpdateRating(ratingVM);
                return Ok(updated);
            }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(); }
        }

        [HttpGet("ocenyZestawuPytan")]
        public async Task<IActionResult> GetRatingsByQuestionsSetId(
            [FromQuery] int questionsSetId)
        {
            try
            {
                var ratings =
                    await _dataService.GetRatingsByQuestionsSetId(questionsSetId);
                return Ok(ratings);
            }
            catch(DataNotFoundException e) { return NotFound(e.Message); }
            catch(Exception e) { return BadRequest(e.Message); }
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
            catch (DataNotFoundException e){ return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
        }
        #endregion

        #region Diagnoza
        [HttpGet("diagnozy")]
        public async Task<IEnumerable<DiagnosisViewModel>> GetAllDiagnosis()
        {
            var diagnosis = await _dataService.GetAllDiagnosis();
            return diagnosis;
        }

        [HttpGet("diagnozy/{id}")]
        public async Task<IActionResult> GetDiagnosisById(int id)
        {
            try
            {
                var diagnosis = await _dataService.GetDiagnosisById(id);
                return Ok(diagnosis);
            }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
        }


        [HttpPost("diagnozy")]
        public async Task<IActionResult> AddDiagnosis(CreateDiagnosisDto createDiagnosis)
        {
            try
            {
                var diagnosis = await _dataService.AddDiagnosis(createDiagnosis);
                return CreatedAtAction(nameof(GetDiagnosisById),
                    new { id = diagnosis.Id }, diagnosis);
            }
            catch (DataValidationException e) { return BadRequest(e.Message); }
            catch (DataNotFoundException e) { return NotFound(); }
            catch (Exception e) { return BadRequest(e.Message); }
        }
        #endregion

        #region Wyniki diagnozy
        [HttpGet("wyniki/{id}")]
        public async Task<IActionResult> GetResultById(long id)
        {
            try
            {
                var result = await _dataService.GetResultById(id);
                return Ok(result);
            }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
        }

        [HttpPost("wyniki")]
        public async Task<IActionResult> AddDiagnosisResult(CreateResultDto createResult)
        {
            try
            {
                var result = await _dataService.AddDiagnosisResult(createResult);
                return CreatedAtRoute(nameof(GetResultById),
                    new { id = result.Id }, result);
            }
            catch (DataValidationException e) { return BadRequest(e.Message); }
            catch (DataNotFoundException e) { return NotFound(); }
            catch (Exception e) { return BadRequest(e.Message); }
        }

        [HttpGet("wyniki")]
        public async Task<IActionResult> GetResultByDiagnosisQuestionsSetIds(
            [FromQuery] int diagnosisId, [FromQuery] int questionsSetId)
        {
            try
            {
                var result = await _dataService.GetResultByDiagnosisQuestionsSetIds(
                    diagnosisId, questionsSetId);
                return Ok(result);
            }
            catch (DataNotFoundException e) { return NotFound(); }
            catch (Exception e) { return BadRequest(e.Message); }
        }
        #endregion
    }
}
