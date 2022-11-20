using Microsoft.EntityFrameworkCore;
using Quiz.Data.DataAccess;
using Quiz.Data.Helpers;
using Quiz.Data.Models;
using Quiz.Data.Models.Base;
using Quiz.Infrastructure.Interfaces;
using Quiz.Shared.DTOs;
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

        public async Task<IEnumerable<EmployeeViewModel>> GetAllEmployees() =>
            await _dbContext.Pracownicy.Select(p => new EmployeeViewModel
            {
                Id = p.Id,
                FirstName = p.Imie,
                LastName = p.Nazwisko,
                DateOfBirth = p.DataUrodzenia,
                PersonalNumber = p.Pesel,
                Salary = p.Pensja,
                Email = p.Email,
                Job = p.Etat.Nazwa,
                Position = p.Stanowisko.Nazwa,
                DateOfEmployment = p.DataZatrudnienia
            })
            .ToListAsync();

        public async Task<IEnumerable<StudentViewModel>> GetAllStudents() =>
            await _dbContext.Uczniowie.Select(u => new StudentViewModel
            {
                Id = u.Id,
                FirstName = u.Imie,
                LastName = u.Nazwisko,
                DateOfBirth = u.DataUrodzenia,
                PersonalNumber = u.Pesel,
                Branch = u.Oddzial.Nazwa
            })
            .ToListAsync();

        #region QuestionsSet
        public async Task<IEnumerable<QuestionsSetViewModel>> GetAllQuestionsSets() =>
            await _dbContext.ZestawyPytan.Select(z => new QuestionsSetViewModel
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
