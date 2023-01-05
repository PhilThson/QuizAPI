using Microsoft.EntityFrameworkCore;
using Quiz.Data.DataAccess;
using Quiz.Data.Helpers;
using Quiz.Data.Models;
using Quiz.Data.Models.Base;
using Quiz.Infrastructure.Interfaces;
using Quiz.Shared.DTOs;
using Quiz.Shared.DTOs.Read;
using Quiz.Shared.ViewModels;

namespace Quiz.Infrastructure.Services
{
    public class DataService : IDataService
    {
        private readonly QuizDbContext _dbContext;

        public DataService(QuizDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Employees
        //lista wszystkich elementów zwraca tylko podstawowe dane
        public async Task<IEnumerable<EmployeeViewModel>> GetAllEmployees() =>
            await _dbContext.Pracownicy
            .Select(p => new EmployeeViewModel
            {
                Id = p.Id,
                FirstName = p.Imie,
                LastName = p.Nazwisko,
                DateOfBirth = p.DataUrodzenia,
                PersonalNumber = p.Pesel
            })
            .ToListAsync();

        public async Task<EmployeeViewModel> GetEmployeeById(int id) =>
            await _dbContext.Pracownicy
            .Where(p => p.Id == id)
            .Select(p => new EmployeeViewModel
            {
                Id = p.Id,
                FirstName = p.Imie,
                LastName = p.Nazwisko,
                DateOfBirth = p.DataUrodzenia,
                PersonalNumber = p.Pesel,
                Salary = p.Pensja,
                Email = p.Email,
                PhoneNumber = p.NrTelefonu,
                Job = new JobDto()
                {
                    Id = p.Etat.Id,
                    Name = p.Etat.Nazwa
                },
                Position = new PositionDto()
                {
                    Id = p.Stanowisko.Id,
                    Name = p.Stanowisko.Nazwa
                },
                DateOfEmployment = p.DataZatrudnienia
            })
            .FirstOrDefaultAsync()
            ?? throw new DataNotFoundException();

        public async Task<EmployeeViewModel> AddEmployee(CreateEmployeeDto employeeDto)
        {
            if (!_dbContext.Etaty
                .Any(e => e.Id == employeeDto.JobId))
                throw new DataNotFoundException("Nie znaleziono etatu o podanym" +
                    $"identyfikatorze ({employeeDto.JobId})");

            if (!_dbContext.Stanowiska
                .Any(s => s.Id == employeeDto.PositionId))
                throw new DataNotFoundException("Nie znaleziono stanowiska " +
                    $"o podanym identyfikatorze ({employeeDto.PositionId})");

            var employeeToCreate = (Pracownik)employeeDto;
            await _dbContext.Pracownicy.AddAsync(employeeToCreate);
            await _dbContext.SaveChangesAsync();

            return await GetEmployeeById(employeeToCreate.Id);
        }

        public async Task DeleteEmployeeById(int id)
        {
            var employeeToDelete = await _dbContext.Pracownicy
                .FirstOrDefaultAsync(e => e.Id == id) ??
                throw new DataNotFoundException();

            employeeToDelete.CzyAktywny = false;
            await _dbContext.SaveChangesAsync();
        }
        #endregion

        #region Students
        public async Task<IEnumerable<StudentViewModel>> GetAllStudents() =>
            await _dbContext.Uczniowie
            .Select(u => new StudentViewModel
            {
                Id = u.Id,
                FirstName = u.Imie,
                LastName = u.Nazwisko,
                DateOfBirth = u.DataUrodzenia,
                PersonalNumber = u.Pesel
            })
            .ToListAsync();

        public async Task<StudentViewModel> GetStudentById(int id) =>
            await _dbContext.Uczniowie
            .Select(u => new StudentViewModel
            {
                Id = u.Id,
                FirstName = u.Imie,
                LastName = u.Nazwisko,
                DateOfBirth = u.DataUrodzenia,
                PersonalNumber = u.Pesel,
                Branch = new BranchDto
                {
                    Id = u.Oddzial.Id,
                    Name = u.Oddzial.Nazwa,
                    Description = u.Oddzial.Opis,
                    Teacher = u.Oddzial.Pracownik.Imie + " " +
                        u.Oddzial.Pracownik.Nazwisko
                }
            })
            .FirstOrDefaultAsync()
            ?? throw new DataNotFoundException();

        public async Task<StudentViewModel> AddStudent(CreateStudentDto studentDto)
        {
            if (!_dbContext.Oddzialy.Any(o => o.Id == studentDto.BranchId))
                throw new DataNotFoundException("Nie znaleziono oddziału " +
                    $"o podanym identyfikatorze: {studentDto.BranchId}");

            var student = (Uczen)studentDto;
            await _dbContext.Uczniowie.AddAsync(student);
            await _dbContext.SaveChangesAsync();

            return await GetStudentById(student.Id);
        }

        public async Task DeleteStudentById(int id)
        {
            var studentToDelete = await _dbContext.Uczniowie
                .FirstOrDefaultAsync(u => u.Id == id) ??
                throw new DataNotFoundException();

            studentToDelete.CzyAktywny = false;
            await _dbContext.SaveChangesAsync();
        }

        #endregion

        #region QuestionsSet
        public async Task<IEnumerable<QuestionsSetViewModel>> GetAllQuestionsSets() =>
            await _dbContext.ZestawyPytan
            .Select(z => new QuestionsSetViewModel
            {
                Id = z.Id,
                SkillDescription = z.OpisUmiejetnosci,
                Area = new AreaViewModel
                {
                    Id = z.ObszarZestawuPytan.Id,
                    Name = z.ObszarZestawuPytan.Nazwa,
                    ExtendedName = z.ObszarZestawuPytan.NazwaRozszerzona,
                    Description = z.ObszarZestawuPytan.Opis
                },
                Difficulty = new DifficultyViewModel
                {
                    Id = z.SkalaTrudnosci.Id,
                    Name = z.SkalaTrudnosci.Nazwa,
                    Description = z.SkalaTrudnosci.Opis
                },
                Questions = new List<QuestionViewModel>
                    (
                        z.ZestawPytanPytania
                        .Select(p => new QuestionViewModel
                        {
                            Id = p.Id,
                            Content = p.Tresc,
                            Description = p.Opis,
                            QuestionsSetId = p.ZestawPytanId
                        })
                    ),
                QuestionsSetRatings = new List<RatingViewModel>
                    (
                        z.ZestawPytanOceny
                        .Select(o => new RatingViewModel
                        {
                            Id = o.Id,
                            RatingDescription = o.OpisOceny,
                            QuestionsSetId = o.ZestawPytanId
                        })
                    ),
                Attachments = new List<AttachmentViewModel>
                    (
                        z.ZestawPytanKartyPracy
                        .Select(kp => new AttachmentViewModel
                        {
                            Id = kp.Id,
                            Name = kp.Nazwa,
                            Description = kp.Opis,
                            ContentType = kp.RodzajZawartosci,
                            Size = kp.Rozmiar,
                            QuestionsSetId = kp.ZestawPytanId
                        })
                    )
            })
            .ToListAsync();

        public async Task<QuestionsSetViewModel> GetQuestionsSetById(int id) =>
            await _dbContext.ZestawyPytan
            .Where(z => z.Id == id)
            .Select(z => new QuestionsSetViewModel
            {
                Id = z.Id,
                SkillDescription = z.OpisUmiejetnosci,
                Area = new AreaViewModel
                {
                    Id = z.ObszarZestawuPytan.Id,
                    Name = z.ObszarZestawuPytan.Nazwa,
                    ExtendedName = z.ObszarZestawuPytan.NazwaRozszerzona,
                    Description = z.ObszarZestawuPytan.Opis
                },
                Difficulty = new DifficultyViewModel
                {
                    Id = z.SkalaTrudnosci.Id,
                    Name = z.SkalaTrudnosci.Nazwa,
                    Description = z.SkalaTrudnosci.Opis
                },
                Questions = new List<QuestionViewModel>
                (
                    z.ZestawPytanPytania.Select(p => new QuestionViewModel
                    {
                        Id = p.Id,
                        Content = p.Tresc,
                        Description = p.Opis,
                        QuestionsSetId = p.ZestawPytanId
                    })
                ),
                QuestionsSetRatings = new List<RatingViewModel>
                    (
                        z.ZestawPytanOceny
                        .Select(o => new RatingViewModel
                        {
                            Id = o.Id,
                            RatingDescription = o.OpisOceny,
                            QuestionsSetId = o.ZestawPytanId
                        })
                    ),
                Attachments = new List<AttachmentViewModel>
                    (
                        z.ZestawPytanKartyPracy
                        .Select(kp => new AttachmentViewModel
                        {
                            Id = kp.Id,
                            Name = kp.Nazwa,
                            Description = kp.Opis,
                            ContentType = kp.RodzajZawartosci,
                            Size = kp.Rozmiar,
                            QuestionsSetId = kp.ZestawPytanId
                        })
                    )
            })
            .FirstOrDefaultAsync()
            ?? throw new DataNotFoundException();

        public async Task<QuestionsSetViewModel> AddQuestionsSet(
            CreateQuestionsSetDto createQuestionsSet)
        {
            if (createQuestionsSet.AreaId == default(byte) ||
                createQuestionsSet.DifficultyId == default(byte))
                throw new DataValidationException();

            if (!_dbContext.ObszaryZestawowPytan
                .Any(o => o.Id == createQuestionsSet.AreaId))
                throw new DataNotFoundException("Nie znaleziono obszaru o podanym" +
                    $"identyfikatorze ({createQuestionsSet.AreaId})");

            if (!_dbContext.SkaleTrudnosci
                .Any(o => o.Id == createQuestionsSet.DifficultyId))
                throw new DataNotFoundException("Nie znaleziono skali trudności " +
                    $"o podanym identyfikatorze ({createQuestionsSet.DifficultyId})");

            var questionsSet = new ZestawPytan
            {
                OpisUmiejetnosci = createQuestionsSet.SkillDescription,
                ObszarZestawuPytanId = createQuestionsSet.AreaId,
                SkalaTrudnosciId = createQuestionsSet.DifficultyId,
                CzyAktywny = true
            };
            await _dbContext.AddAsync(questionsSet);
            await _dbContext.SaveChangesAsync();

            if(createQuestionsSet.QuestionsSetRatings.Count() > 0)
            {
                var questionsSetRatings = new List<OcenaZestawuPytan>();
                foreach (var rating in createQuestionsSet.QuestionsSetRatings)
                {
                    questionsSetRatings.Add(new OcenaZestawuPytan
                    {
                        OpisOceny = rating,
                        ZestawPytanId = questionsSet.Id,
                        CzyAktywny = true
                    });
                }
                await _dbContext.AddRangeAsync(questionsSetRatings);
                await _dbContext.SaveChangesAsync();
            }

            if (createQuestionsSet.Questions?.Count() > 0)
            {
                var questions = new List<Pytanie>();
                foreach (var question in createQuestionsSet.Questions)
                    questions.Add(new Pytanie
                    {
                        Tresc = question.Content,
                        Opis = question.Description,
                        ZestawPytanId = questionsSet.Id,
                        CzyAktywny = true
                    });
                await _dbContext.Pytania.AddRangeAsync(questions);
                await _dbContext.SaveChangesAsync();
            }

            if (createQuestionsSet.AttachmentFiles?.Count() > 0)
            {
                var attFiles = new List<KartaPracy>();
                foreach(var attFile in createQuestionsSet.AttachmentFiles)
                    attFiles.Add(new KartaPracy
                    {
                        Nazwa = attFile.Name,
                        Opis = attFile.Description,
                        RodzajZawartosci = attFile.ContentType,
                        Zawartosc = attFile.Content,
                        Rozmiar = attFile.Size,
                        ZestawPytanId = questionsSet.Id,
                        CzyAktywny = true
                    });
                await _dbContext.KartyPracy.AddRangeAsync(attFiles);
                await _dbContext.SaveChangesAsync();
            }

            return await GetQuestionsSetById(questionsSet.Id);
        }

        public async Task<AreaViewModel> UpdateQuestionsSetArea(int id, byte areaId)
        {
            if (!_dbContext.ObszaryZestawowPytan.Any(o => o.Id == areaId))
                throw new DataNotFoundException("Nie znaleziono obszaru o podanym" +
                    $"identyfikatorze ({areaId})");

            var questionsSetFromDb = await _dbContext.ZestawyPytan
                .Where(z => z.Id == id)
                .FirstOrDefaultAsync() ?? throw new DataNotFoundException();

            if (questionsSetFromDb.ObszarZestawuPytanId == areaId)
                return await GetAreaById(questionsSetFromDb.ObszarZestawuPytanId);

            questionsSetFromDb.ObszarZestawuPytanId = areaId;
            await _dbContext.SaveChangesAsync();
            return await GetAreaById(areaId);
        }

        public async Task<DifficultyViewModel> UpdateQuestionsSetDifficulty(int id,
            byte difficultyId)
        {
            if (!_dbContext.SkaleTrudnosci.Any(o => o.Id == difficultyId))
                throw new DataNotFoundException("Nie znaleziono skali trudności " +
                    $"o podanym identyfikatorze ({difficultyId})");

            var questionsSetFromDb = await _dbContext.ZestawyPytan
                .Where(z => z.Id == id)
                .FirstOrDefaultAsync() ?? throw new DataNotFoundException();

            if (questionsSetFromDb.SkalaTrudnosciId == difficultyId)
                return await GetDifficultyById(questionsSetFromDb.SkalaTrudnosciId);

            questionsSetFromDb.SkalaTrudnosciId = difficultyId;
            await _dbContext.SaveChangesAsync();
            return await GetDifficultyById(difficultyId);
        }
        #endregion

        #region Attachment
        public async Task<AttachmentFileViewModel> GetAttachmentById(int id) =>
            await _dbContext.KartyPracy
            .Where(k => k.Id == id)
            .Select(k => new AttachmentFileViewModel
            {
                Id = k.Id,
                Name = k.Nazwa,
                Description = k.Opis,
                Content = k.Zawartosc,
                ContentType = k.RodzajZawartosci,
                Size = k.Rozmiar
            })
            .FirstOrDefaultAsync()
            ?? throw new DataNotFoundException();
        #endregion

        #region Question
        public async Task<IEnumerable<QuestionViewModel>> GetAllQuestions() =>
            await _dbContext.Pytania
            .Select(p => new QuestionViewModel
            {
                Id = p.Id,
                Content = p.Tresc,
                Description = p.Opis,
                QuestionsSetId = p.ZestawPytanId
            })
            .ToListAsync();

        public async Task<QuestionViewModel> GetQuestionById(int id) =>
            await _dbContext.Pytania
            .Where(p => p.Id == id)
            .Select(p => new QuestionViewModel
            {
                Id = p.Id,
                Content = p.Tresc,
                Description = p.Opis,
                QuestionsSetId = p.ZestawPytanId
            })
            .FirstOrDefaultAsync()
            ?? throw new DataNotFoundException();

        public async Task<QuestionViewModel> AddQuestion(QuestionViewModel questionVM)
        {
            if (questionVM.QuestionsSetId == default(int))
                throw new DataValidationException("Pytanie musi być częścią" +
                    "wybranego zestawu pytań");

            var question = new Pytanie
            {
                Tresc = questionVM.Content,
                Opis = questionVM.Description,
                ZestawPytanId = questionVM.QuestionsSetId
            };

            await _dbContext.Pytania.AddAsync(question);
            await _dbContext.SaveChangesAsync();

            return await GetQuestionById(question.Id);
        }

        public async Task<QuestionViewModel> UpdateQuestion(QuestionViewModel questionVM)
        {
            if (questionVM.QuestionsSetId == default(int))
                throw new DataValidationException();

            var questionFromDb = await _dbContext.Pytania
                .FirstOrDefaultAsync(p => p.Id == questionVM.Id);

            _ = questionFromDb ?? throw new DataNotFoundException();

            questionFromDb.Tresc = questionVM.Content;
            questionFromDb.Opis = questionVM.Description;
            questionFromDb.ZestawPytanId = questionVM.QuestionsSetId;

            _dbContext.Pytania.Attach(questionFromDb);
            await _dbContext.SaveChangesAsync();

            return await GetQuestionById(questionFromDb.Id);
        }
        #endregion

        #region SkillDescription
        public async Task<string> UpdateSkillDescription(int id, string skill)
        {
            var questionsSetFromDb = await _dbContext.ZestawyPytan
                .FirstOrDefaultAsync(p => p.Id == id);

            _ = questionsSetFromDb ?? throw new DataNotFoundException();

            if (questionsSetFromDb.OpisUmiejetnosci == skill)
                return questionsSetFromDb.OpisUmiejetnosci;

            questionsSetFromDb.OpisUmiejetnosci = skill;

            await _dbContext.SaveChangesAsync();

            return questionsSetFromDb.OpisUmiejetnosci;
        }
        #endregion

        #region QuestionsSet Area
        public async Task<IEnumerable<AreaViewModel>> GetAllAreas() =>
            await _dbContext.ObszaryZestawowPytan
            .Select(o => new AreaViewModel
            {
                Id = o.Id,
                Name = o.Nazwa,
                Description = o.Opis,
                ExtendedName = o.NazwaRozszerzona
            })
            .ToListAsync();

        public async Task<AreaViewModel> GetAreaById(byte id) =>
            await _dbContext.ObszaryZestawowPytan
            .Where(o => o.Id == id)
            .Select(o => new AreaViewModel
            {
                Id = o.Id,
                Name = o.Nazwa,
                Description = o.Opis,
                ExtendedName = o.NazwaRozszerzona
            })
            .FirstOrDefaultAsync()
            ?? throw new DataNotFoundException();

        public async Task<AreaViewModel> AddArea(CreateDictionaryDto createDto)
        {
            var existingArea = await _dbContext.ObszaryZestawowPytan
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(a => a.Nazwa == createDto.Name
                                && a.Opis == createDto.Description);

            if (existingArea != null)
            {
                if (existingArea.CzyAktywny)
                    throw new AlreadyExistsException();

                existingArea.CzyAktywny = true;
                return await GetAreaById(existingArea.Id);
            }

            var areaToCreate = new ObszarZestawuPytan
            {
                Nazwa = createDto.Name,
                Opis = createDto.Description,
                CzyAktywny = true
            };
            await _dbContext.ObszaryZestawowPytan.AddAsync(areaToCreate);
            await _dbContext.SaveChangesAsync();

            return await GetAreaById(areaToCreate.Id);
        }

        public async Task<AreaViewModel> UpdateArea(AreaViewModel areaVM)
        {
            var areaFromDb = await _dbContext.ObszaryZestawowPytan
                .FirstOrDefaultAsync(p => p.Id == areaVM.Id);

            _ = areaFromDb ?? throw new DataNotFoundException();

            areaFromDb.Nazwa = areaVM.Name;
            areaFromDb.Opis = areaVM.Description;
            areaFromDb.NazwaRozszerzona = areaVM.ExtendedName;

            await _dbContext.SaveChangesAsync();

            return await GetAreaById(areaFromDb.Id);
        }

        public async Task DeleteAreaById(byte id)
        {
            var area = await _dbContext.ObszaryZestawowPytan
                .FirstOrDefaultAsync(a => a.Id == id) ??
                throw new DataNotFoundException();

            area.CzyAktywny = false;
            await _dbContext.SaveChangesAsync();
        }
        #endregion

        #region Difficulty
        public async Task<IEnumerable<DifficultyViewModel>> GetAllDifficulties() =>
            await _dbContext.SkaleTrudnosci
            .Select(s => new DifficultyViewModel
            {
                Id = s.Id,
                Name = s.Nazwa,
                Description = s.Opis
            })
            .ToListAsync();

        public async Task<DifficultyViewModel> GetDifficultyById(byte id) =>
            await _dbContext.SkaleTrudnosci
            .Where(s => s.Id == id)
            .Select(s => new DifficultyViewModel
            {
                Id = s.Id,
                Name = s.Nazwa,
                Description = s.Opis
            })
            .FirstOrDefaultAsync()
            ?? throw new DataNotFoundException();

        public async Task<DifficultyViewModel> AddDifficulty(CreateDictionaryDto createDto)
        {
            var existingDifficulty = await _dbContext.SkaleTrudnosci
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(d => d.Nazwa == createDto.Name
                                && d.Opis == createDto.Description);
            if (existingDifficulty != null)
            {
                if (existingDifficulty.CzyAktywny)
                    throw new AlreadyExistsException();

                existingDifficulty.CzyAktywny = true;
                return await GetDifficultyById(existingDifficulty.Id);
            }

            var difficultyToCreate = new SkalaTrudnosci
            {
                Nazwa = createDto.Name,
                Opis = createDto.Description,
                CzyAktywny = true,
            };
            await _dbContext.SkaleTrudnosci.AddAsync(difficultyToCreate);
            await _dbContext.SaveChangesAsync();

            return await GetDifficultyById(difficultyToCreate.Id);
        }

        public async Task<DifficultyViewModel> UpdateDifficulty(
            DifficultyViewModel difficultyVM)
        {
            var difficultyFromDb = await _dbContext.SkaleTrudnosci
                .FirstOrDefaultAsync(p => p.Id == difficultyVM.Id);

            _ = difficultyFromDb ?? throw new DataNotFoundException();

            if (difficultyFromDb.Nazwa == difficultyVM.Name &&
                difficultyFromDb.Opis == difficultyVM.Description)
                return difficultyVM;

            difficultyFromDb.Nazwa = difficultyVM.Name;
            difficultyFromDb.Opis = difficultyVM.Description;

            await _dbContext.SaveChangesAsync();

            return await GetDifficultyById(difficultyFromDb.Id);
        }

        public async Task DeleteDifficultyById(byte id)
        {
            var difficulty = await _dbContext.SkaleTrudnosci
                .FirstOrDefaultAsync(d => d.Id == id) ??
                throw new DataNotFoundException();

            difficulty.CzyAktywny = false;
            await _dbContext.SaveChangesAsync();
        }
        #endregion

        #region Rating
        public async Task<RatingViewModel> GetRatingById(int id) =>
            await _dbContext.OcenyZestawowPytan
                .Where(o => o.Id == id)
                .Select(o => new RatingViewModel
                {
                    Id = o.Id,
                    RatingDescription = o.OpisOceny,
                    QuestionsSetId = o.ZestawPytanId
                })
                .FirstOrDefaultAsync()
                ?? throw new DataNotFoundException();
    
        public async Task<RatingViewModel> UpdateRating(RatingViewModel ratingVM)
        {
            var ratingFromDb = await _dbContext.OcenyZestawowPytan
                .FirstOrDefaultAsync(p => p.Id == ratingVM.Id);

            _ = ratingFromDb ?? throw new DataNotFoundException();
            if (ratingFromDb.OpisOceny == ratingVM.RatingDescription)
                return ratingVM;

            ratingFromDb.OpisOceny = ratingVM.RatingDescription;
            await _dbContext.SaveChangesAsync();

            return await GetRatingById(ratingFromDb.Id);
        }

        public async Task<List<RatingViewModel>> GetRatingsByQuestionsSetId(
            int questionsSetId)
        {
            if (!_dbContext.ZestawyPytan.Any(z => z.Id == questionsSetId))
                throw new DataNotFoundException("Nie znaleziono zestawu " +
                    $"pytań o podanym identyfikatorze ({questionsSetId})");

            return await _dbContext.OcenyZestawowPytan
            .Where(o => o.ZestawPytanId == questionsSetId)
            .Select(o => new RatingViewModel
            {
                Id = o.Id,
                RatingDescription = o.OpisOceny,
                QuestionsSetId = o.ZestawPytanId
            })
            .ToListAsync();
        }
        #endregion

        #region Diagnosis
        public async Task<IEnumerable<DiagnosisViewModel>> GetAllDiagnosis() =>
            await _dbContext.Diagnozy
            .Select(d => new DiagnosisViewModel
            {
                Id = d.Id,
                Employee = new EmployeeViewModel
                {
                    Id = d.Pracownik.Id,
                    FirstName = d.Pracownik.Imie,
                    LastName = d.Pracownik.Nazwisko,
                    DateOfBirth = d.Pracownik.DataUrodzenia,
                    PersonalNumber = d.Pracownik.Pesel,
                    Salary = d.Pracownik.Pensja,
                    Email = d.Pracownik.Email,
                    Job = new JobDto
                    {
                        Id = d.Pracownik.Etat.Id,
                        Name = d.Pracownik.Etat.Nazwa
                    },
                    Position = new PositionDto
                    {
                        Id = d.Pracownik.Stanowisko.Id,
                        Name = d.Pracownik.Stanowisko.Nazwa
                    },
                    DateOfEmployment = d.Pracownik.DataZatrudnienia
                },
                Student = new StudentViewModel
                {
                    Id = d.Uczen.Id,
                    FirstName = d.Uczen.Imie,
                    LastName = d.Uczen.Nazwisko,
                    DateOfBirth = d.Uczen.DataUrodzenia,
                    PersonalNumber = d.Uczen.Pesel
                },
                SchoolYear = d.RokSzkolny,
                Results = new List<ResultViewModel>
                    (
                        d.DiagnozaWyniki
                        .Select(w => new ResultViewModel
                        {
                            Id = w.Id,
                            Notes = w.Notatki,
                            RatingLevel = w.PoziomOceny,
                            QuestionsSetRating = new RatingViewModel
                            {
                                Id = w.OcenaZestawuPytan.Id,
                                RatingDescription = w.OcenaZestawuPytan.OpisOceny,
                                QuestionsSetId = w.OcenaZestawuPytan.ZestawPytanId
                            }
                        })
                    )
            })
            .ToListAsync();

        public async Task<DiagnosisViewModel> GetDiagnosisById(int id) =>
            await _dbContext.Diagnozy
                .Where(d => d.Id == id)
                .Select(d => new DiagnosisViewModel
                {
                    Id = d.Id,
                    Employee = new EmployeeViewModel
                    {
                        Id = d.Pracownik.Id,
                        FirstName = d.Pracownik.Imie,
                        LastName = d.Pracownik.Nazwisko,
                        DateOfBirth = d.Pracownik.DataUrodzenia,
                        PersonalNumber = d.Pracownik.Pesel,
                        Salary = d.Pracownik.Pensja,
                        Email = d.Pracownik.Email,
                        Job = new JobDto()
                        {
                            Id = d.Pracownik.Etat.Id,
                            Name = d.Pracownik.Etat.Nazwa,
                        },
                        Position = new PositionDto()
                        {
                            Id = d.Pracownik.Stanowisko.Id,
                            Name = d.Pracownik.Stanowisko.Nazwa,
                        },
                        DateOfEmployment = d.Pracownik.DataZatrudnienia
                    },
                    Student = new StudentViewModel
                    {
                        Id = d.Uczen.Id,
                        FirstName = d.Uczen.Imie,
                        LastName = d.Uczen.Nazwisko,
                        DateOfBirth = d.Uczen.DataUrodzenia,
                        PersonalNumber = d.Uczen.Pesel
                    },
                    SchoolYear = d.RokSzkolny,
                    Results = new List<ResultViewModel>
                    (
                        d.DiagnozaWyniki
                        .Select(w => new ResultViewModel
                        {
                            Id = w.Id,
                            Notes = w.Notatki,
                            RatingLevel = w.PoziomOceny,
                            QuestionsSetRating = new RatingViewModel
                            {
                                Id = w.OcenaZestawuPytan.Id,
                                RatingDescription = w.OcenaZestawuPytan.OpisOceny,
                                QuestionsSetId = w.OcenaZestawuPytan.ZestawPytanId
                            }
                        })
                    )
                })
                .FirstOrDefaultAsync() ??
                throw new DataNotFoundException();

        public async Task<DiagnosisViewModel> AddDiagnosis(
            CreateDiagnosisDto createDiagnosis)
        {
            if (!_dbContext.Pracownicy
                .Any(p => p.Id == createDiagnosis.EmployeeId))
                throw new DataValidationException(
                    $"Nie znaleziono pracownika o podanym identyfikatorze " +
                    $"({createDiagnosis.EmployeeId})");

            if (!_dbContext.Uczniowie
                .Any(u => u.Id == createDiagnosis.StudentId))
                throw new DataValidationException(
                    $"Nie znaleziono ucznia o podanym identyfikatorze " +
                    $"({createDiagnosis.StudentId})");

            if (_dbContext.Diagnozy
                .Any(d => d.UczenId == createDiagnosis.StudentId &&
                    d.RokSzkolny == createDiagnosis.SchoolYear))
                throw new DataValidationException("Uczeń już posiada diagnozę" +
                    "za dany rok");

            var diagnosis = new Diagnoza
            {
                PracownikId = createDiagnosis.EmployeeId,
                UczenId = createDiagnosis.StudentId,
                RokSzkolny = createDiagnosis.SchoolYear,
                CzyAktywny = true
            };

            await _dbContext.Diagnozy.AddAsync(diagnosis);
            await _dbContext.SaveChangesAsync();

            return await GetDiagnosisById(diagnosis.Id);
        }
        #endregion

        #region Diagnosis Results
        public async Task<ResultViewModel> GetResultById(long id) =>
            await _dbContext.Wyniki
            .Where(w => w.Id == id)
            .Select(w => new ResultViewModel
            {
                Id = w.Id,
                QuestionsSetRating = new RatingViewModel
                {
                    Id = w.OcenaZestawuPytanId,
                    QuestionsSetId = w.OcenaZestawuPytan.ZestawPytanId,
                    RatingDescription = w.OcenaZestawuPytan.OpisOceny
                },
                Notes = w.Notatki,
                RatingLevel = w.PoziomOceny,
                DiagnosisId = w.DiagnozaId
            })
            .FirstOrDefaultAsync() ??
            throw new DataNotFoundException();

        public async Task<ResultViewModel> AddDiagnosisResult(
            CreateResultDto createResult)
        {
            if (!_dbContext.Diagnozy.Any(d => d.Id == createResult.DiagnosisId))
                throw new DataValidationException("Nie znaleziono formularza " +
                    "diagnozy o podanym identyfikatorze " +
                    $"({createResult.DiagnosisId})");

            if (!_dbContext.OcenyZestawowPytan.Any(o => o.Id == createResult.RatingId))
                throw new DataValidationException("Nie znaleziono oceny zestawu " +
                    "pytań o podanym identyfikatorze " +
                    $"({createResult.RatingId})");

            if (createResult.RatingLevel < 1 || createResult.RatingLevel > 6)
                throw new DataValidationException("Przekroczono zakres poziomu oceny " +
                    "zestawu pytań");

            //if (_dbContext.Wyniki
            //    .Any(w => w.DiagnozaId == createResult.DiagnosisId &&
            //        w.OcenaZestawuPytanId == createResult.RatingId))
            //    throw new DataValidationException("Istnieje już " +
            //        "wynik dla podanego zastawu pytań");

            var result = new Wynik
            {
                DiagnozaId = createResult.DiagnosisId,
                OcenaZestawuPytanId = createResult.RatingId,
                PoziomOceny = createResult.RatingLevel,
                Notatki = createResult.Notes,
                CzyAktywny = true
            };

            await _dbContext.Wyniki.AddAsync(result);
            await _dbContext.SaveChangesAsync();

            return await GetResultById(result.Id);
        }


        public async Task<ResultViewModel> GetResultByDiagnosisQuestionsSetIds(
            int diagnosisId, int questionsSetId) =>
            await _dbContext.Wyniki
            .Where(w => w.DiagnozaId == diagnosisId &&
                w.OcenaZestawuPytan.ZestawPytanId == questionsSetId)
            .Select(w => new ResultViewModel
            {
                Id = w.Id,
                QuestionsSetRating = new RatingViewModel
                {
                    Id = w.OcenaZestawuPytan.Id,
                    QuestionsSetId = w.OcenaZestawuPytan.ZestawPytanId,
                    RatingDescription = w.OcenaZestawuPytan.OpisOceny
                },
                Notes = w.Notatki,
                RatingLevel = w.PoziomOceny,
                DiagnosisId = w.DiagnozaId
            })
            .FirstOrDefaultAsync() ??
            throw new DataNotFoundException();
        #endregion

        #region Job
        public async Task<IEnumerable<JobDto>> GetAllJobs() =>
            await _dbContext.Etaty
            .Select(e => new JobDto
            {
                Id = e.Id,
                Name = e.Nazwa
            })
            .ToListAsync();
        #endregion

        #region Position
        public async Task<IEnumerable<PositionDto>> GetAllPositions() =>
            await _dbContext.Stanowiska
            .Select(s => new PositionDto
            {
                Id = s.Id,
                Name = s.Nazwa
            })
            .ToListAsync();
        #endregion

        #region Branch
        public async Task<IEnumerable<BranchDto>> GetAllBranches() =>
            await _dbContext.Oddzialy
            .Select(o => new BranchDto
            {
                Id = o.Id,
                Name = o.Nazwa,
                Description = o.Opis,
                Teacher = o.Pracownik.Imie + " " + o.Pracownik.Nazwisko
            })
            .ToListAsync();
        #endregion

        #region Role
        public async Task<IEnumerable<RoleDto>> GetAllRoles() =>
            await _dbContext.Role
            .Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Nazwa,
                Description = r.Opis
            })
            .ToListAsync();
        #endregion

        #region User
        public async Task<UserDto> GetUserById(int userId) =>
            await _dbContext.Uzytkownicy
            .Where(u => u.Id == userId)
            .Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.Imie,
                LastName = u.Nazwisko,
                Email = u.Email,
                PasswordHash = u.HashHasla,
                Role = new RoleDto
                {
                    Id = u.Rola.Id,
                    Name = u.Rola.Nazwa,
                    Description = u.Rola.Opis
                }
            })
            .FirstOrDefaultAsync() ??
            throw new DataNotFoundException("Nie znaleziono użytkownika " +
                "o podanym identyfikatorze.");

        public async Task<UserDto> GetUserByEmail(string email) =>
            await _dbContext.Uzytkownicy
            .Where(u => u.Email == email)
            .Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.Imie,
                LastName = u.Nazwisko,
                Email = u.Email,
                PasswordHash = u.HashHasla,
                Role = new RoleDto
                {
                    Id = u.Rola.Id,
                    Name = u.Rola.Nazwa,
                    Description = u.Rola.Opis
                }
            })
            .FirstOrDefaultAsync() ??
            throw new DataNotFoundException("Nie znaleziono użytkownika " +
                "o podanym adresie e-mail.");

        public async Task<UserDto> AddUser(CreateUserDto userDto)
        {
            //Na wszelki wypadek, bo już atrybut Required
            if (!userDto.RoleId.HasValue)
                throw new DataValidationException("Użytkownik musi posiadać rolę");

            if (_dbContext.Uzytkownicy.Any(u => u.Email == userDto.Email))
                throw new DataValidationException("Podany adres e-mail " +
                    "jest już zajęty");

            var user = new Uzytkownik
            {
                Imie = userDto.FirstName,
                Nazwisko = userDto.LastName,
                Email = userDto.Email,
                HashHasla = userDto.PasswordHash,
                RolaId = userDto.RoleId.Value,
                CzyAktywny = true
            };

            await _dbContext.Uzytkownicy.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return await GetUserById(user.Id);
        }
        #endregion

        #region Private Methods
        private async Task<TKey> GetObjectId<T, TKey>(string name)
            where T : BaseDictionaryEntity<TKey> =>
            await _dbContext.Set<T>()
                .Where(e => e.Nazwa == name)
                .Select(e => e.Id)
                .FirstOrDefaultAsync() ?? throw new DataNotFoundException();
        #endregion
    }
}
