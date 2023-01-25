using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
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
            try
            {
                var result = await _dataService.GetEmployeeById(id);
                return Ok(result);
            }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
        }

        [HttpPost("pracownicy")]
        public async Task<IActionResult> AddEmployee(CreateEmployeeDto employeeDto)
        {
            try
            {
                var createdEmployee = await _dataService.AddEmployee(employeeDto);
                return CreatedAtAction(nameof(GetEmployeeById),
                    new { id = createdEmployee.Id }, createdEmployee);
            }
            catch (DataValidationException e) { return BadRequest(e.Message); }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
        }

        [HttpDelete("pracownicy/{id}")]
        public async Task<IActionResult> DeleteEmployeeById(int id)
        {
            try
            {
                await _dataService.DeleteEmployeeById(id);
                return NoContent();
            }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
        }

        [HttpPut("pracownicy")]
        public async Task<IActionResult> UpdateEmployee(CreateEmployeeDto employeeDto)
        {
            try
            {
                var updated = await _dataService.UpdateEmployee(employeeDto);
                return Ok(updated);
            }
            catch (DataValidationException e) { return BadRequest(e.Message); }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
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
            try
            {
                var result = await _dataService.GetStudentById(id);
                return Ok(result);
            }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
        }

        [HttpPost("uczniowie")]
        public async Task<IActionResult> AddStudent(CreateStudentDto studentDto)
        {
            try
            {
                var createdStudent = await _dataService.AddStudent(studentDto);
                return CreatedAtAction(nameof(GetStudentById),
                    new { id = createdStudent.Id }, createdStudent);
            }
            catch (DataValidationException e) { return BadRequest(e.Message); }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
        }

        [HttpDelete("uczniowie/{id}")]
        public async Task<IActionResult> DeleteStudentById(int id)
        {
            try
            {
                await _dataService.DeleteStudentById(id);
                return NoContent();
            }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
        }

        [HttpPut("uczniowie")]
        public async Task<IActionResult> UpdateStudent(CreateStudentDto studentDto)
        {
            try
            {
                var updated = await _dataService.UpdateStudent(studentDto);
                return Ok(updated);
            }
            catch (DataValidationException e) { return BadRequest(e.Message); }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
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
            try
            {
                var address = await _dataService.GetAddressById(id);
                return Ok(address);
            }
            catch(DataNotFoundException e) { return NotFound(e.Message); }
            catch(Exception e) { return BadRequest(e.Message); }
        }

        [HttpPost("adresy")]
        public async Task<IActionResult> AddAddress(AddressDto addressDto)
        {
            try
            {
                var createdAddress = await _dataService.AddAddress(addressDto);
                return CreatedAtAction(nameof(GetAddressById),
                    new { id = createdAddress.Id }, createdAddress);
            }
            catch (DataValidationException e) { return BadRequest(e.Message); }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
        }

        [HttpDelete("adresy/{id}")]
        public async Task<IActionResult> DeleteAddressById([FromRoute] int id)
        {
            try
            {
                await _dataService.DeleteAddressById(id);
                return NoContent();
            }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
        }

        [HttpPut("adresy")]
        public async Task<IActionResult> UpdateAddress(AddressDto addressDto)
        {
            try
            {
                var updated = await _dataService.UpdateAddress(addressDto);
                return Ok(updated);
            }
            catch (DataValidationException e) { return BadRequest(e.Message); }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
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
            catch(DataNotFoundException e) { return NotFound(e.Message); }
        }

        [HttpPost("pytania")]
        public async Task<IActionResult> AddQuestion(QuestionViewModel questionVM)
        {
            try
            {
                if (questionVM.Id != default(int))
                    return BadRequest("Przesłany obiekt nie może posiadać identyfikatora");

                var question = await _dataService.AddQuestion(questionVM);

                return CreatedAtAction(nameof(GetQuestionById),
                    new { id = question.Id}, question);
            }
            catch (DataValidationException e) { return BadRequest(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
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
            catch (Exception e) { return BadRequest(e.Message); }
        }


        [HttpDelete("pytania/{id}")]
        public async Task<IActionResult> DeleteQuestionById(int id)
        {
            try
            {
                await _dataService.DeleteQuestionById(id);
                return NoContent();
            }
            catch (DataNotFoundException e) { return BadRequest(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
        }
        #endregion

        #region Zestawy pytań
        [HttpGet("zestawyPytan")]
        public async Task<IActionResult> GetAllQuestionsSets([FromQuery] byte? difficultyId)
        {
            try
            {
                if (!difficultyId.HasValue)
                    return Ok(await _dataService.GetQuestionsSetsByCondition(zp => zp.CzyAktywny));
                else
                {
                    var difficulty =
                        await _dataService.GetDifficultyById(difficultyId.Value);

                    //wersja aktualna, czyli np.:
                    //dla skali BC zwróci B, C oraz BC
                    var questionsSets = await _dataService.GetQuestionsSetsByCondition(
                        zp => difficulty.Name.Contains(zp.SkalaTrudnosci.Nazwa) &&
                        zp.CzyAktywny);

                    return Ok(questionsSets);
                }
            }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
        }

        [HttpGet("zestawyPytan/zadane")]
        public async Task<IActionResult> GetQuestionsSetsByIds(
            [FromQuery] string askedQuestionSetsIds)
        {
            try
            {
                var listOfIds = askedQuestionSetsIds?.Split(',')?.Select(Int32.Parse)?.ToList();
                //Tutaj nie ma filtrowania aktywnych zestawów pytań, ponieważ musi być edycja
                //i podgląd ZP wchodzących w skład istniejąych diagnoz
                var questionsSets = await _dataService.GetQuestionsSetsByCondition(
                    zp => listOfIds.Contains(zp.Id));
                return Ok(questionsSets);
            }
            catch (Exception e) { return BadRequest(e.Message); }
        }

        [HttpGet("zestawyPytan/{id}", Name = nameof(GetQuestionsSetById))]
        public async Task<IActionResult> GetQuestionsSetById([FromRoute] int id)
        {
            try
            {
                var questionsSet = await _dataService.GetQuestionsSetById(id);
                return Ok(questionsSet);
            }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
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
            catch (Exception e) { return BadRequest(e.Message); }
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
            catch (Exception e) { return BadRequest(e.Message); }
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
            catch (Exception e) { return BadRequest(e.Message); }
        }

        [HttpDelete("zestawyPytan/{id}")]
        public async Task<IActionResult> DeleteQuestionsSetById(int id)
        {
            try
            {
                await _dataService.DeleteQuestionsSetById(id);
                return NoContent();
            }
            catch (DataNotFoundException e) { return BadRequest(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
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
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
        }

        [HttpPost("obszary")]
        public async Task<IActionResult> AddArea(CreateDictionaryDto createDto)
        {
            try
            {
                var created = await _dataService.AddArea(createDto);
                return CreatedAtAction(nameof(GetAreaById),
                    new { id = created.Id }, created);
            }
            catch (AlreadyExistsException e) { return BadRequest(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
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
            catch (Exception e) { return BadRequest(e.Message); }
        }

        [HttpDelete("obszary/{id}")]
        public async Task<IActionResult> DeleteAreaById(byte id)
        {
            try
            {
                await _dataService.DeleteAreaById(id);
                return NoContent();
            }
            catch (DataNotFoundException e) { return BadRequest(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
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
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
        }

        [HttpPost("skaleTrudnosci")]
        public async Task<IActionResult> AddDifficulty(CreateDictionaryDto createDto)
        {
            try
            {
                var created = await _dataService.AddDifficulty(createDto);
                return CreatedAtAction(nameof(GetDifficultyById),
                    new { id = created.Id }, created);
            }
            catch (AlreadyExistsException e) { return BadRequest(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
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
            catch (Exception e) { return BadRequest(e.Message); }
        }

        [HttpDelete("skaleTrudnosci/{id}")]
        public async Task<IActionResult> DeleteDifficultyById([FromRoute] byte id)
        {
            try
            {
                await _dataService.DeleteDifficultyById(id);
                return NoContent();
            }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
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
            catch (Exception e) { return BadRequest(e.Message); }
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
            catch (DataNotFoundException e) { return NotFound(e.Message); }
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
                return CreatedAtAction(nameof(GetResultById),
                    new { id = result.Id }, result);
            }
            catch (DataValidationException e) { return BadRequest(e.Message); }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
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
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
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

        #region Role
        [HttpGet("role")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _dataService.GetAllRoles();
            return Ok(roles);
        }
        #endregion

        #region Użytkownicy
        [HttpGet("uzytkownicy/{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] int userId)
        {
            try
            {
                var user = await _dataService.GetUserById(userId);
                return Ok(user);
            }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
        }

        [HttpGet("uzytkownicy")]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
        {
            try
            {
                var user = await _dataService.GetUserByEmail(email);
                return Ok(user);
            }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
        }

        [HttpPost("uzytkownicy")]
        public async Task<IActionResult> AddUser([FromBody] CreateUserDto userDto)
        {
            try
            {
                var user = await _dataService.AddUser(userDto);
                return CreatedAtAction(nameof(GetUserById),
                    new { id = user.Id }, user);
            }
            catch (DataValidationException e) { return BadRequest(e.Message); }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
        }
        #endregion

        #region Raporty
        [HttpPost("raporty/{diagnosisId}")]
        public async Task<IActionResult> GenerateDiagnosisReport([FromRoute] int diagnosisId)
        {
            try
            {
                var diagnosis = await _dataService.GetDiagnosisById(diagnosisId);

                if (diagnosis.ReportId.HasValue)
                    return Ok(await _dataService.GetReportById(diagnosis.ReportId.Value));

                var baseReport = await _dataService.AddDiagnosisReport(diagnosis);

                return Ok(baseReport);
            }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (DataValidationException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
        }

        [HttpGet("raporty/{reportId}")]
        public async Task<IActionResult> GetDiagnosisReport([FromRoute] int reportId)
        {
            try
            {
                var report = await _dataService.GetReportById(reportId);
                return Ok(report);
            }
            catch (DataNotFoundException e) { return NotFound(e.Message); }
            catch (DataValidationException e) { return NotFound(e.Message); }
            catch (Exception e) { return BadRequest(e.Message); }
        }
        #endregion
    }
}
