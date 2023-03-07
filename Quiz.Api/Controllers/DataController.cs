using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Quiz.Api.Filters;
using Quiz.Data.Helpers;
using Quiz.Infrastructure;
using Quiz.Infrastructure.Interfaces;
using Quiz.Shared.DTOs;
using Quiz.Shared.ViewModels;

namespace Quiz.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        #region Pola prywatne
        private readonly IDataService _dataService;
        #endregion

        #region Konstruktor
        public DataController(IDataService dataService)
        {
            _dataService = dataService;
        }
        #endregion

        #region Pracownicy
        [HttpGet("pracownicy")]
        public async Task<IEnumerable<EmployeeViewModel>> GetAllEmployees()
        {
            var employees = await _dataService.GetAllEmployees();
            return employees;
        }

        [HttpGet("pracownicy/{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var result = await _dataService.GetEmployeeById(id);
            return Ok(result);
        }

        [HttpPost("pracownicy")]
        public async Task<IActionResult> AddEmployee(CreateEmployeeDto employeeDto)
        {
            var createdEmployee = await _dataService.AddEmployee(employeeDto);
            return CreatedAtAction(nameof(GetEmployeeById),
                new { id = createdEmployee.Id }, createdEmployee);
        }

        [HttpDelete("pracownicy/{id}")]
        public async Task<IActionResult> DeleteEmployeeById(int id)
        {
            await _dataService.DeleteEmployeeById(id);
            return NoContent();
        }

        [HttpPut("pracownicy")]
        public async Task<IActionResult> UpdateEmployee(CreateEmployeeDto employeeDto)
        {
            var updated = await _dataService.UpdateEmployee(employeeDto);
            return Ok(updated);
        }
        #endregion

        #region Uczniowie
        [HttpGet("uczniowie")]
        public async Task<IEnumerable<StudentViewModel>> GetAllStudents()
        {
            var students = await _dataService.GetAllStudents();
            return students;
        }

        [HttpGet("uczniowie/{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var result = await _dataService.GetStudentById(id);
            return Ok(result);
        }

        [HttpPost("uczniowie")]
        public async Task<IActionResult> AddStudent(CreateStudentDto studentDto)
        {
            var createdStudent = await _dataService.AddStudent(studentDto);
            return CreatedAtAction(nameof(GetStudentById),
                new { id = createdStudent.Id }, createdStudent);
        }

        [HttpDelete("uczniowie/{id}")]
        public async Task<IActionResult> DeleteStudentById(int id)
        {
            await _dataService.DeleteStudentById(id);
            return NoContent();
        }

        [HttpPut("uczniowie")]
        public async Task<IActionResult> UpdateStudent(CreateStudentDto studentDto)
        {
            var updated = await _dataService.UpdateStudent(studentDto);
            return Ok(updated);
        }
        #endregion

        #region Adresy
        [HttpGet("adresy")]
        public async Task<IActionResult> GetAllAddresses()
        {
            var addresses = await _dataService.GetAllAddresses();
            return Ok(addresses);
        }

        [HttpGet("adresy/{id}")]
        public async Task<IActionResult> GetAddressById([FromRoute] int id)
        {
            var address = await _dataService.GetAddressById(id);
            return Ok(address);
        }

        [HttpPost("adresy")]
        public async Task<IActionResult> AddAddress(AddressDto addressDto)
        {
            var createdAddress = await _dataService.AddAddress(addressDto);
            return CreatedAtAction(nameof(GetAddressById),
                new { id = createdAddress.Id }, createdAddress);
        }

        [HttpDelete("adresy/{id}")]
        public async Task<IActionResult> DeleteAddressById([FromRoute] int id)
        {
            await _dataService.DeleteAddressById(id);
            return NoContent();
        }

        [HttpPut("adresy")]
        public async Task<IActionResult> UpdateAddress(AddressDto addressDto)
        {
            var updated = await _dataService.UpdateAddress(addressDto);
            return Ok(updated);
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
            var question = await _dataService.GetQuestionById(id);
            return Ok(question);
        }

        [HttpPost("pytania")]
        public async Task<IActionResult> AddQuestion(QuestionViewModel questionVM)
        {
            if (questionVM.Id != default(int))
                return BadRequest("Przesłany obiekt nie może posiadać identyfikatora");

            var question = await _dataService.AddQuestion(questionVM);

            return CreatedAtAction(nameof(GetQuestionById),
                new { id = question.Id }, question);
        }

        [HttpPut("pytania")]
        public async Task<IActionResult> UpdateQuestion(QuestionViewModel questionVM)
        {
            var question = await _dataService.UpdateQuestion(questionVM);
            return Ok(question);
        }


        [HttpDelete("pytania/{id}")]
        public async Task<IActionResult> DeleteQuestionById(int id)
        {
            await _dataService.DeleteQuestionById(id);
            return NoContent();
        }
        #endregion

        #region Zestawy pytań
        [HttpGet("zestawyPytan")]
        public async Task<IActionResult> GetAllQuestionsSets([FromQuery] byte? difficultyId,
            [FromQuery] string? askedQuestionSetsIds)
        {
            if (difficultyId.HasValue)
            {
                var difficulty = await _dataService.GetDifficultyById(difficultyId.Value);

                //wersja aktualna, czyli np.:
                //dla skali BC zwróci B, C oraz BC
                var questionsSets = await _dataService.GetQuestionsSetsByCondition(
                    zp => difficulty.Name.Contains(zp.SkalaTrudnosci.Nazwa) &&
                        zp.CzyAktywny);

                return Ok(questionsSets);
            }

            if (!string.IsNullOrEmpty(askedQuestionSetsIds))
            {
                var listOfIds = askedQuestionSetsIds?.Split(',')?.Select(Int32.Parse)?.ToList();
                //Tutaj nie ma filtrowania aktywnych zestawów pytań, ponieważ musi być edycja
                //i podgląd ZP wchodzących w skład istniejąych diagnoz
                var questionsSets = await _dataService.GetQuestionsSetsByCondition(
                    zp => listOfIds.Contains(zp.Id));

                return Ok(questionsSets);
            }

            return Ok(await _dataService.GetQuestionsSetsByCondition(zp => zp.CzyAktywny));
        }

        [HttpGet("zestawyPytan/{id}", Name = nameof(GetQuestionsSetById))]
        public async Task<IActionResult> GetQuestionsSetById([FromRoute] int id)
        {
            var questionsSet = await _dataService.GetQuestionsSetById(id);
            return Ok(questionsSet);
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
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
        }

        [HttpPatch("zestawyPytan/{id}")]
        public async Task<IActionResult> UpdateQuestionsSetProperty([FromRoute] int id,
            [FromBody] KeyValuePair<string, string> propertyValue)
        {
            if (string.IsNullOrEmpty(propertyValue.Key))
                throw new DataValidationException(
                    "Przesłany model nie zawiera wartości do aktualizacji");

            switch (propertyValue.Key)
            {
                case "skill":
                    if (propertyValue.Value.Length > 2048)
                        throw new DataValidationException(
                            "Przesłano opis umiejętności musi mieć poniżej 2048 znaków");

                    var updated =
                        await _dataService.UpdateSkillDescription(id, propertyValue.Value);

                    return Ok(updated);

                case "area":
                    if (!byte.TryParse(propertyValue.Value, out byte areaId))
                        throw new DataValidationException(
                            "Przesłano nieprawidłowy identyfikator obszaru");

                    if (areaId == default(byte))
                        throw new DataNotFoundException();

                    var updatedArea = await _dataService.UpdateQuestionsSetArea(id, areaId);

                    return Ok(updatedArea);

                case "difficulty":
                    if (!byte.TryParse(propertyValue.Value, out byte difficultyId))
                        throw new DataValidationException(
                            "Przesłano nieprawidłowy identyfikator skali trudności");

                    if (difficultyId == default(byte))
                        throw new DataNotFoundException();

                    var updatedDifficulty =
                        await _dataService.UpdateQuestionsSetDifficulty(id, difficultyId);

                    return Ok(updatedDifficulty);

                default:
                    return BadRequest("Nie znaleziono właściwości do aktualizacji");
            }
        }

        [HttpDelete("zestawyPytan/{id}")]
        public async Task<IActionResult> DeleteQuestionsSetById(int id)
        {
            await _dataService.DeleteQuestionsSetById(id);
            return NoContent();
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
            var area = await _dataService.GetAreaById(id);
            return Ok(area);
        }

        [HttpPost("obszary")]
        public async Task<IActionResult> AddArea(CreateDictionaryDto createDto)
        {
            var created = await _dataService.AddArea(createDto);
            return CreatedAtAction(nameof(GetAreaById),
                new { id = created.Id }, created);
        }

        [HttpPut("obszary")]
        public async Task<IActionResult> UpdateArea([FromBody] AreaViewModel areaVM)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var updated = await _dataService.UpdateArea(areaVM);
            return Ok(updated);
        }

        [HttpDelete("obszary/{id}")]
        public async Task<IActionResult> DeleteAreaById(byte id)
        {
            await _dataService.DeleteAreaById(id);
            return NoContent();
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
            var difficulty = await _dataService.GetDifficultyById(id);
            return Ok(difficulty);
        }

        [HttpPost("skaleTrudnosci")]
        public async Task<IActionResult> AddDifficulty(CreateDictionaryDto createDto)
        {
            var created = await _dataService.AddDifficulty(createDto);
            return CreatedAtAction(nameof(GetDifficultyById),
                new { id = created.Id }, created);
        }

        [HttpPut("skaleTrudnosci")]
        public async Task<IActionResult> UpdateDifficulty([FromBody]
            DifficultyViewModel difficultyVM)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var updated = await _dataService.UpdateDifficulty(difficultyVM);
            return Ok(updated);
        }

        [HttpDelete("skaleTrudnosci/{id}")]
        public async Task<IActionResult> DeleteDifficultyById([FromRoute] byte id)
        {
            await _dataService.DeleteDifficultyById(id);
            return NoContent();
        }
        #endregion

        #region Oceny zestawu pytań
        [HttpGet("ocenyZestawuPytan/{id}")]
        public async Task<IActionResult> GetRatingById([FromRoute] int id)
        {
            var rating = await _dataService.GetRatingById(id);
            return Ok(rating);
        }

        [HttpPut("ocenyZestawuPytan")]
        public async Task<IActionResult> UpdateRating([FromBody] RatingViewModel ratingVM)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var updated = await _dataService.UpdateRating(ratingVM);
            return Ok(updated);
        }

        [HttpGet("ocenyZestawuPytan")]
        public async Task<IActionResult> GetRatingsByQuestionsSetId(
            [FromQuery] int questionsSetId)
        {
            var ratings =
                await _dataService.GetRatingsByQuestionsSetId(questionsSetId);
            return Ok(ratings);
        }
        #endregion

        #region Karty pracy
        [HttpGet("kartyPracy/{id}", Name = nameof(GetAttachmentById))]
        public async Task<IActionResult> GetAttachmentById([FromRoute] int id)
        {
            var attachment = await _dataService.GetAttachmentById(id);
            return Ok(attachment);
        }
        #endregion

        #region Diagnozy
        [HttpGet("diagnozy")]
        public async Task<IEnumerable<DiagnosisViewModel>> GetAllDiagnosis()
        {
            var diagnosis = await _dataService.GetAllDiagnosis();
            return diagnosis;
        }

        [HttpGet("diagnozy/{id}")]
        public async Task<IActionResult> GetDiagnosisById(int id)
        {
            var diagnosis = await _dataService.GetDiagnosisById(id);
            return Ok(diagnosis);
        }


        [HttpPost("diagnozy")]
        public async Task<IActionResult> AddDiagnosis(CreateDiagnosisDto createDiagnosis)
        {
            var diagnosis = await _dataService.AddDiagnosis(createDiagnosis);
            return CreatedAtAction(nameof(GetDiagnosisById),
                new { id = diagnosis.Id }, diagnosis);
        }
        #endregion

        #region Wyniki diagnozy
        [HttpGet("wyniki/{id}")]
        public async Task<IActionResult> GetResultById(long id)
        {
            var result = await _dataService.GetResultById(id);
            return Ok(result);
        }

        [HttpPost("wyniki")]
        public async Task<IActionResult> AddDiagnosisResult(CreateResultDto createResult)
        {
            var result = await _dataService.AddDiagnosisResult(createResult);
            return CreatedAtAction(nameof(GetResultById),
                new { id = result.Id }, result);
        }

        [HttpGet("wyniki")]
        public async Task<IActionResult> GetResultByDiagnosisQuestionsSetIds(
            [FromQuery] int diagnosisId, [FromQuery] int questionsSetId)
        {
            var result = await _dataService.GetResultByDiagnosisQuestionsSetIds(
                diagnosisId, questionsSetId);
            return Ok(result);
        }
        #endregion

        #region Etaty
        [HttpGet("etaty")]
        public async Task<IActionResult> GetAllJobs()
        {
            var jobs = await _dataService.GetAllJobs();
            return Ok(jobs);
        }
        #endregion

        #region Stanowiska
        [HttpGet("stanowiska")]
        public async Task<IActionResult> GetAllPositions()
        {
            var positions = await _dataService.GetAllPositions();
            return Ok(positions);
        }
        #endregion

        #region Oddzialy
        [HttpGet("oddzialy")]
        public async Task<IActionResult> GetAllBranches()
        {
            var branches = await _dataService.GetAllBranches();
            return Ok(branches);
        }
        #endregion

        #region Raporty
        [HttpPost("raporty/{diagnosisId}")]
        public async Task<IActionResult> GenerateDiagnosisReport([FromRoute] int diagnosisId)
        {
            var diagnosis = await _dataService.GetDiagnosisById(diagnosisId);

            if (diagnosis.ReportId.HasValue)
                return Ok(await _dataService.GetReportById(diagnosis.ReportId.Value));

            var baseReport = await _dataService.AddDiagnosisReport(diagnosis);

            return Ok(baseReport);
        }

        [HttpGet("raporty/{reportId}")]
        public async Task<IActionResult> GetDiagnosisReport([FromRoute] int reportId)
        {
            var report = await _dataService.GetReportById(reportId);
            return Ok(report);
        }
        #endregion
    }
}
