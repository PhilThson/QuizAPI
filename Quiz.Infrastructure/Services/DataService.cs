using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Quiz.Data.DataAccess;
using Quiz.Data.Helpers;
using Quiz.Data.Models;
using Quiz.Data.Models.Base;
using Quiz.Infrastructure.Interfaces;
using Quiz.Infrastructure.Helpers;
using Quiz.Shared.DTOs;
using Quiz.Shared.DTOs.Read;
using Quiz.Shared.ViewModels;
using System.Net;

namespace Quiz.Infrastructure.Services
{
    public class DataService : IDataService
    {
        #region Private fields
        private readonly QuizDbContext _dbContext;
        //private readonly IDocumentService _documentService;
        #endregion

        #region Constructor
        public DataService(QuizDbContext dbContext
            //, IDocumentService documentService
            )
        {
            _dbContext = dbContext;
            //_documentService = documentService;
        }
        #endregion

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
                Salary = p.Pensja,
                Email = p.Email,
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
                DaysOfLeave = p.DniUrlopu,
                HourlyRate = p.WymiarGodzinowy,
                Overtime = p.Nadgodziny,
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
                DateOfEmployment = p.DataZatrudnienia,
                EmploymentEndDate = p.DataKoncaZatrudnienia,
                Addresses = new List<AddressDto>
                (
                    p.PracownikPracownicyAdresy
                    .Select(a => new AddressDto
                    {
                        Id = a.Adres.Id,
                        Country = a.Adres.Panstwo,
                        City = a.Adres.Miejscowosc,
                        Street = a.Adres.Ulica,
                        HouseNumber = a.Adres.NumerDomu,
                        FlatNumber = a.Adres.NumerMieszkania,
                        PostalCode = a.Adres.KodPocztowy
                    })
                )
            })
            .FirstOrDefaultAsync()
            ?? throw new DataNotFoundException();

        public async Task<EmployeeViewModel> AddEmployee(CreateEmployeeDto employeeDto)
        {
            var existingEmployee = await _dbContext.Pracownicy
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(p =>
                    p.Imie == employeeDto.FirstName &&
                    p.Nazwisko == employeeDto.LastName &&
                    p.Pesel == employeeDto.PersonalNumber
                );

            if (existingEmployee is not null)
            {
                if (existingEmployee.CzyAktywny)
                    throw new AlreadyExistsException("Wybrany pracownik już isnieje");

                existingEmployee.CzyAktywny = true;
                await _dbContext.SaveChangesAsync();
                return await GetEmployeeById(existingEmployee.Id);
            }

            if (!_dbContext.Etaty
                .Any(e => e.Id == employeeDto.JobId))
                throw new DataNotFoundException("Nie znaleziono etatu o podanym" +
                    $"identyfikatorze ({employeeDto.JobId})");

            if (!_dbContext.Stanowiska
                .Any(s => s.Id == employeeDto.PositionId))
                throw new DataNotFoundException("Nie znaleziono stanowiska " +
                    $"o podanym identyfikatorze ({employeeDto.PositionId})");

            ValidatePersonalNumber(employeeDto.PersonalNumber);

            var employeeToCreate = (Pracownik)employeeDto;

            await _dbContext.Pracownicy.AddAsync(employeeToCreate);

            if (employeeDto.AddressesIds?.Count > 0)
            {
                foreach (var addressId in employeeDto.AddressesIds)
                {
                    _dbContext.PracownicyAdresy.Add(
                        new PracownicyAdresy
                        {
                            Pracownik = employeeToCreate,
                            AdresId = addressId
                        });
                }
            }

            await _dbContext.SaveChangesAsync();

            return await GetEmployeeById(employeeToCreate.Id);
        }

        public async Task DeleteEmployeeById(int id)
        {
            var employeeToDelete = await _dbContext.Pracownicy
                .Include(p => p.PracownikPracownicyAdresy)
                .FirstOrDefaultAsync(e => e.Id == id) ??
                throw new DataNotFoundException();

            //nie są usuwane adresy pracownika, ponieważ jest to zachowanie
            //historii pracownika
            //if (employeeToDelete.PracownikPracownicyAdresy.Count() > 0)
            //    _dbContext.PracownicyAdresy.RemoveRange
            //        (employeeToDelete.PracownikPracownicyAdresy);

            employeeToDelete.CzyAktywny = false;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<EmployeeViewModel> UpdateEmployee(CreateEmployeeDto employeeDto)
        {
            var employee = await _dbContext.Pracownicy
                .FirstOrDefaultAsync(p => p.Id == employeeDto.Id) ??
                throw new DataNotFoundException(
                    $"Nie znaleziono pracownika o podanym identyfikatorze ({employeeDto.Id})");

            if (employee == employeeDto)
                return await GetEmployeeById(employee.Id);

            if (!_dbContext.Etaty
                .Any(e => e.Id == employeeDto.JobId))
                throw new DataNotFoundException("Nie znaleziono etatu o podanym" +
                    $"identyfikatorze ({employeeDto.JobId})");

            if (!_dbContext.Stanowiska
                .Any(s => s.Id == employeeDto.PositionId))
                throw new DataNotFoundException("Nie znaleziono stanowiska " +
                    $"o podanym identyfikatorze ({employeeDto.PositionId})");

            ValidatePersonalNumber(employeeDto.PersonalNumber);

            employee.FillEmployeeModel(employeeDto);
            await _dbContext.SaveChangesAsync();

            return await GetEmployeeById(employee.Id);
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
                PlaceOfBirth = u.MiejsceUrodzenia,
                DisabilityCert = u.NrOrzeczenia,
                PersonalNumber = u.Pesel
            })
            .ToListAsync();

        public async Task<StudentViewModel> GetStudentById(int id) =>
            await _dbContext.Uczniowie
            .Where(u => u.Id == id)
            .Select(u => new StudentViewModel
            {
                Id = u.Id,
                FirstName = u.Imie,
                LastName = u.Nazwisko,
                DateOfBirth = u.DataUrodzenia,
                PlaceOfBirth = u.MiejsceUrodzenia,
                DisabilityCert = u.NrOrzeczenia,
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
            var existingStudent = await _dbContext.Uczniowie
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u =>
                    u.Imie == studentDto.FirstName &&
                    u.Nazwisko == studentDto.LastName &&
                    u.Pesel == studentDto.PersonalNumber
                );

            if(existingStudent is not null)
            {
                if (existingStudent.CzyAktywny)
                    throw new AlreadyExistsException("Wybrany uczeń już isnieje");

                existingStudent.CzyAktywny = true;
                await _dbContext.SaveChangesAsync();
                return await GetStudentById(existingStudent.Id);
            }

            if (!_dbContext.Oddzialy.Any(o => o.Id == studentDto.BranchId))
                throw new DataNotFoundException("Nie znaleziono oddziału " +
                    $"o podanym identyfikatorze: {studentDto.BranchId}");

            ValidatePersonalNumber(studentDto.PersonalNumber);

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

        public async Task<StudentViewModel> UpdateStudent(CreateStudentDto studentDto)
        {
            var student = await _dbContext.Uczniowie
                .FirstOrDefaultAsync(u => u.Id == studentDto.Id) ??
                throw new DataNotFoundException(
                    $"Nie znaleziono ucznia o podanym identyfikatorze ({studentDto.Id})");

            if (student == studentDto)
                return await GetStudentById(student.Id);

            if (!_dbContext.Oddzialy.Any(o => o.Id == studentDto.BranchId))
                throw new DataValidationException("Nie znaleziono oddziału " +
                    $"o podanym identyfikatorze: {studentDto.BranchId}");

            ValidatePersonalNumber(studentDto.PersonalNumber);

            student.FillStudentModel(studentDto);
            await _dbContext.SaveChangesAsync();

            return await GetStudentById(student.Id);
        }

        #endregion

        #region Addresses

        public async Task<IEnumerable<AddressDto>> GetAllAddresses() =>
            await _dbContext.Adresy
            .Select(a => new AddressDto
            {
                Id = a.Id,
                Country = a.Panstwo,
                City = a.Miejscowosc,
                Street = a.Ulica,
                HouseNumber = a.NumerDomu,
                FlatNumber = a.NumerMieszkania,
                PostalCode = a.KodPocztowy
            })
            .ToListAsync();

        public async Task<AddressDto> GetAddressById(int id) =>
            await _dbContext.Adresy
            .Where(a => a.Id == id)
            .Select(a => new AddressDto
            {
                Id = a.Id,
                Country = a.Panstwo,
                City = a.Miejscowosc,
                Street = a.Ulica,
                HouseNumber = a.NumerDomu,
                FlatNumber = a.NumerMieszkania,
                PostalCode = a.KodPocztowy
            })
            .FirstOrDefaultAsync() ??
            throw new DataNotFoundException(
                $"Nie znaleziono adresu o podanym identyfikatorze ({id})");

        public async Task<AddressDto> AddAddress(AddressDto addressDto)
        {
            var existingAddress = await _dbContext.Adresy
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(a =>
                    a.Panstwo == addressDto.Country &&
                    a.Miejscowosc == addressDto.City &&
                    a.Ulica == addressDto.Street &&
                    a.NumerDomu == addressDto.HouseNumber &&
                    a.NumerMieszkania == addressDto.FlatNumber &&
                    a.KodPocztowy == addressDto.PostalCode);

            if(existingAddress is not null)
            {
                if (existingAddress.CzyAktywny)
                    throw new AlreadyExistsException(
                        "Adres o podanych paramterach już istnieje");

                existingAddress.CzyAktywny = true;
                await _dbContext.SaveChangesAsync();
                return await GetAddressById(existingAddress.Id);
            }

            var address = (Adres)addressDto;
            await _dbContext.Adresy.AddAsync(address);
            await _dbContext.SaveChangesAsync();

            return await GetAddressById(address.Id);
        }

        public async Task<AddressDto> UpdateAddress(AddressDto addressDto)
        {
            var address = await _dbContext.Adresy
                .FirstOrDefaultAsync(a => a.Id == addressDto.Id) ??
                throw new DataNotFoundException(
                    $"Nie znaleziono adresu o podanym identyfikatorze ({addressDto.Id})");

            if (address == addressDto)
                return await GetAddressById(address.Id);

            address.FillAddressModel(addressDto);
            await _dbContext.SaveChangesAsync();

            return await GetAddressById(address.Id);
        }

        public async Task DeleteAddressById(int id)
        {
            var address = await _dbContext.Adresy
                .Include(a => a.AdresPracownicyAdresy)
                //.ThenInclude(pa => pa.Pracownik)
                .FirstOrDefaultAsync(a => a.Id == id) ??
                throw new DataNotFoundException(
                    $"Nie znaleziono adresu o podanym identyfikatorze ({id})");

            if(address.AdresPracownicyAdresy.Count() > 0)
                _dbContext.PracownicyAdresy.RemoveRange(address.AdresPracownicyAdresy);

            address.CzyAktywny = false;
            await _dbContext.SaveChangesAsync();
        }

        #endregion

        #region QuestionsSets
        public async Task<IEnumerable<QuestionsSetViewModel>>
            GetQuestionsSetsByCondition(
                Expression<Func<ZestawPytan, bool>>? filter = null)
        {
            var query = _dbContext.ZestawyPytan.AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            return await query.Select(z => new QuestionsSetViewModel
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
                //przy pobieraniu zestawów pytań, NIE pobiera się
                //zawartości załączników, tylko podstawowe informacje o nich
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
        }

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

            if (createQuestionsSet.QuestionsSetRatings.Count() > 0)
                foreach (var rating in createQuestionsSet.QuestionsSetRatings)
                    questionsSet.ZestawPytanOceny.Add(new OcenaZestawuPytan
                    {
                        OpisOceny = rating,
                        CzyAktywny = true
                    });

            //Obecnie nie ma możliwości dodania pytania podczas tworzenia zestawu pytań
            //if (createQuestionsSet.Questions?.Count() > 0)
            //{
            //    foreach (var question in createQuestionsSet.Questions)
            //        questionsSet.ZestawPytanPytania.Add(new Pytanie
            //        {
            //            Tresc = question.Content,
            //            Opis = question.Description,
            //            CzyAktywny = true
            //        });
            //}

            if (createQuestionsSet.AttachmentFiles?.Count() > 0)
                foreach (var attFile in createQuestionsSet.AttachmentFiles)
                    questionsSet.ZestawPytanKartyPracy.Add(new KartaPracy
                    {
                        Nazwa = attFile.Name,
                        Opis = attFile.Description,
                        RodzajZawartosci = attFile.ContentType,
                        Zawartosc = attFile.Content,
                        Rozmiar = attFile.Size,
                        ZestawPytanId = questionsSet.Id,
                        CzyAktywny = true
                    });


            await _dbContext.AddAsync(questionsSet);
            await _dbContext.SaveChangesAsync();

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

        public async Task DeleteQuestionsSetById(int id)
        {
            var questionsSet = await _dbContext.ZestawyPytan
                .Include(zp => zp.ZestawPytanPytania)
                .Include(zp => zp.ZestawPytanOceny)
                .Include(zp => zp.ZestawPytanKartyPracy)
                .FirstOrDefaultAsync(zp => zp.Id == id && zp.CzyAktywny) ??
                throw new DataNotFoundException(
                    "Nie znaleziono zestawu pytań o podanym identyfikatorze");

            foreach (var question in questionsSet.ZestawPytanPytania)
                question.CzyAktywny = false;

            foreach (var rating in questionsSet.ZestawPytanOceny)
                rating.CzyAktywny = false;

            foreach (var attachment in questionsSet.ZestawPytanKartyPracy)
                attachment.CzyAktywny = false;

            questionsSet.CzyAktywny = false;
            await _dbContext.SaveChangesAsync();
        }
        #endregion

        #region Attachments
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
            .FirstOrDefaultAsync() ??
            throw new DataNotFoundException(
                $"Nie znaleziono karty pracy o podanym identyfikatorze ({id})");
        #endregion

        #region Questions
        public async Task<IEnumerable<QuestionViewModel>> GetAllQuestions() =>
            await _dbContext.Pytania
            .Where(p => p.CzyAktywny)
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
            .FirstOrDefaultAsync() ??
            throw new DataNotFoundException(
                $"Nie znaleziono pytania o podanym identyfikatorze ({id})");

        public async Task<QuestionViewModel> AddQuestion(QuestionViewModel questionVM)
        {
            if (questionVM.QuestionsSetId == default(int))
                throw new DataValidationException("Pytanie musi być częścią" +
                    "wybranego zestawu pytań");

            var question = new Pytanie
            {
                Tresc = questionVM.Content,
                Opis = questionVM.Description,
                ZestawPytanId = questionVM.QuestionsSetId,
                CzyAktywny = true
            };

            await _dbContext.Pytania.AddAsync(question);
            await _dbContext.SaveChangesAsync();

            return await GetQuestionById(question.Id);
        }

        public async Task<QuestionViewModel> UpdateQuestion(QuestionViewModel questionVM)
        {
            if (questionVM.QuestionsSetId == default(int))
                throw new DataValidationException(
                    "Pytanie musi być przypisane do zestawu pytań");

            var questionFromDb = await _dbContext.Pytania
                .FirstOrDefaultAsync(p => p.Id == questionVM.Id) ??
                throw new DataNotFoundException();

            if(questionFromDb == questionVM)
                await GetQuestionById(questionFromDb.Id);

            questionFromDb.Tresc = questionVM.Content;
            questionFromDb.Opis = questionVM.Description;
            questionFromDb.ZestawPytanId = questionVM.QuestionsSetId;

            _dbContext.Pytania.Attach(questionFromDb);
            await _dbContext.SaveChangesAsync();

            return await GetQuestionById(questionFromDb.Id);
        }

        public async Task DeleteQuestionById(int id)
        {
            var question = await _dbContext.Pytania
                .FirstOrDefaultAsync(p => p.Id == id) ??
                    throw new DataNotFoundException("Nie znaleziono pytania " +
                        $"o podanym identyfikatorze ({id})");

            question.CzyAktywny = false;
            await _dbContext.SaveChangesAsync();
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

        #region Areas
        public async Task<IEnumerable<AreaViewModel>> GetAllAreas() =>
            await _dbContext.ObszaryZestawowPytan
            .Where(o => o.CzyAktywny)
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
                .FirstOrDefaultAsync(a => a.Nazwa == createDto.Name &&
                                a.Opis == createDto.Description);

            if (existingArea != null)
            {
                if (existingArea.CzyAktywny)
                    throw new AlreadyExistsException();

                existingArea.CzyAktywny = true;
                await _dbContext.SaveChangesAsync();
                return await GetAreaById(existingArea.Id);
            }

            var areaToCreate = new ObszarZestawuPytan
            {
                Nazwa = createDto.Name,
                Opis = createDto.Description,
                NazwaRozszerzona = createDto.ExtendedName,
                CzyAktywny = true
            };
            await _dbContext.ObszaryZestawowPytan.AddAsync(areaToCreate);
            await _dbContext.SaveChangesAsync();

            return await GetAreaById(areaToCreate.Id);
        }

        public async Task<AreaViewModel> UpdateArea(AreaViewModel areaVM)
        {
            var areaFromDb = await _dbContext.ObszaryZestawowPytan
                .FirstOrDefaultAsync(p => p.Id == areaVM.Id) ??
                throw new DataNotFoundException();

            if (areaFromDb == areaVM)
                return areaVM;

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

        #region Difficulties
        public async Task<IEnumerable<DifficultyViewModel>> GetAllDifficulties() =>
            await _dbContext.SkaleTrudnosci
            .Where(s => s.CzyAktywny)
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
                await _dbContext.SaveChangesAsync();
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
                .FirstOrDefaultAsync(p => p.Id == difficultyVM.Id) ??
                throw new DataNotFoundException();

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

        #region Ratings
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
                .FirstOrDefaultAsync(p => p.Id == ratingVM.Id) ??
                throw new DataNotFoundException();

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
                Institution = d.PlacowkaOswiatowa,
                CounselingCenter = d.PoradniaPsychologiczna,
                Difficulty = new DifficultyViewModel
                {
                    Id = d.DiagnozaSkalaTrudnosci.Id,
                    Name = d.DiagnozaSkalaTrudnosci.Nazwa,
                    Description = d.DiagnozaSkalaTrudnosci.Opis
                },
                //Nie są zwracane rezultaty diagnozy na liście wszystkich diagnoz
                CreatedDate = d.DataPrzeprowadzenia,
                ReportId = d.DiagnozaRaport.Id
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
                        PlaceOfBirth = d.Uczen.MiejsceUrodzenia,
                        DisabilityCert = d.Uczen.NrOrzeczenia,
                        PersonalNumber = d.Uczen.Pesel
                    },
                    SchoolYear = d.RokSzkolny,
                    Institution = d.PlacowkaOswiatowa,
                    CounselingCenter = d.PoradniaPsychologiczna,
                    Difficulty = new DifficultyViewModel
                    {
                        Id = d.DiagnozaSkalaTrudnosci.Id,
                        Name = d.DiagnozaSkalaTrudnosci.Nazwa,
                        Description = d.DiagnozaSkalaTrudnosci.Opis
                    },
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
                    ),
                    CreatedDate = d.DataPrzeprowadzenia,
                    ReportId = d.DiagnozaRaport.Id,
                })
                .FirstOrDefaultAsync() ??
                throw new DataNotFoundException("Nie znaleziono diagnozy o podanym " +
                    $"identyfikatorze {id}");

        public async Task<DiagnosisViewModel> AddDiagnosis(
            CreateDiagnosisDto createDiagnosis)
        {
            if (string.IsNullOrEmpty(createDiagnosis.Institution))
                throw new DataValidationException(
                    "Należy podać pełną nazwę placówki oświatowej");

            if (string.IsNullOrEmpty(createDiagnosis.CounselingCenter))
                throw new DataValidationException(
                    "Należy podać pełną nazwę PPP");

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

            if (!_dbContext.SkaleTrudnosci
                .Any(s => s.Id == createDiagnosis.DifficultyId))
                throw new DataValidationException(
                    "Nie znaleziono skali trudności o podanym identyfikatorze " +
                    $"({createDiagnosis.DifficultyId})");

            if (_dbContext.Diagnozy
                .Any(d => d.UczenId == createDiagnosis.StudentId &&
                    d.RokSzkolny == createDiagnosis.SchoolYear))
                throw new DataValidationException("Uczeń już posiada diagnozę" +
                    "za dany rok");

            var diagnosis = (Diagnoza)createDiagnosis;

            await _dbContext.Diagnozy.AddAsync(diagnosis);
            await _dbContext.SaveChangesAsync();

            return await GetDiagnosisById(diagnosis.Id);
        }

        public async Task<BaseReportDto> AddDiagnosisReport(DiagnosisViewModel diagnosis)
        {
            if (_dbContext.Raporty.Any(r => r.DiagnozaId == diagnosis.Id))
                throw new DataValidationException("Diagnoza już posiada wygenerowany raport");

            var diagnosisToPdf = await GetDiagnosisToPdfViewModel(diagnosis);
            //Zamockowanie do developersko na macOS'ie:
            //zakomentować + dodać pustą tablicę bajtów i rozmiar
            //var pdfDocument = _documentService
            //    .GeneratePdfFromRazorView("/Views/DiagnosisSummary.cshtml", diagnosisToPdf);

            var report = new Raport
            {
                Nazwa = $"{diagnosis.Id}_" +
                    $"{diagnosisToPdf.Employee.LastName}_" +
                    $"{diagnosisToPdf.Student.LastName}_" +
                    $"{diagnosisToPdf.SchoolYear}.pdf",
                //Zawartosc = pdfDocument,
                Zawartosc = new byte[100],
                //Rozmiar = pdfDocument.Length,
                Rozmiar = 100,
                DiagnozaId = diagnosis.Id,
                CzyAktywny = true
            };

            await _dbContext.Raporty.AddAsync(report);
            await _dbContext.SaveChangesAsync();

            return await GetBaseReportById(report.Id);
        }

        private async Task<DiagnosisToPdfViewModel> GetDiagnosisToPdfViewModel(
            DiagnosisViewModel diagnosis)
        {
            var askedQuestionSetsIds = new List<int>();

            if (diagnosis.Results?.Count > 0)
                askedQuestionSetsIds = diagnosis.Results
                    .Select(r => r.QuestionsSetRating.QuestionsSetId).ToList();

            var questionsSets = await GetQuestionsSetsByCondition(
                    zp => askedQuestionSetsIds.Contains(zp.Id));

            var masteredQSIds = diagnosis.Results.Where(d => d.RatingLevel > 4)
                .Select(r => r.QuestionsSetRating.QuestionsSetId).ToList();
            var toImproveQSIds = diagnosis.Results.Where(d => d.RatingLevel < 5)
                .Select(r => r.QuestionsSetRating.QuestionsSetId).ToList();

            var diagnosisToPdf = (DiagnosisToPdfViewModel)diagnosis;

            diagnosisToPdf.QuestionsSetsMastered =
                    questionsSets.Where(qs => masteredQSIds.Contains(qs.Id))
                    .OrderBy(qs => qs.Area.Name).ToList();
            diagnosisToPdf.QuestionsSetsToImprove =
                    questionsSets.Where(qs => toImproveQSIds.Contains(qs.Id))
                    .OrderBy(qs => qs.Area.Name).ToList();

            return diagnosisToPdf;
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

            //Obecnie można edytować istniejący wynik
            //if (_dbContext.Wyniki
            //    .Any(w => w.DiagnozaId == createResult.DiagnosisId &&
            //        w.OcenaZestawuPytanId == createResult.RatingId))
            //    throw new DataValidationException("Istnieje już " +
            //        "wynik dla podanego zastawu pytań");

            var result = (Wynik)createResult;

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
                Name = e.Nazwa,
                Description = e.Opis
            })
            .ToListAsync();
        #endregion

        #region Position
        public async Task<IEnumerable<PositionDto>> GetAllPositions() =>
            await _dbContext.Stanowiska
            .Select(s => new PositionDto
            {
                Id = s.Id,
                Name = s.Nazwa,
                Description = s.Opis
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

        #region Users
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

        #region Reports
        public async Task<ReportDto> GetReportById(int reportId) =>
            await _dbContext.Raporty
            .Where(r => r.Id == reportId)
            .Select(r => new ReportDto
            {
                Id = r.Id,
                Name = r.Nazwa,
                Description = r.Opis,
                Content = r.Zawartosc,
                Size = r.Rozmiar
            })
            .FirstOrDefaultAsync() ??
            throw new DataNotFoundException("Nie znaleziono raportu " +
                $"o podanym identyfikatorze ({reportId})");

        private async Task<BaseReportDto> GetBaseReportById(int reportId) =>
            await _dbContext.Raporty
            .Where(r => r.Id == reportId)
            .Select(r => new BaseReportDto
            {
                Id = r.Id,
                Name = r.Nazwa,
                Description = r.Opis,
                Size = r.Rozmiar
            })
            .FirstOrDefaultAsync() ??
            throw new DataNotFoundException("Nie znaleziono raportu " +
                $"o podanym identyfikatorze ({reportId})");
        #endregion

        #region Private Methods
        private async Task<TKey> GetObjectId<T, TKey>(string name)
            where T : BaseDictionaryEntity<TKey> =>
            await _dbContext.Set<T>()
                .Where(e => e.Nazwa == name)
                .Select(e => e.Id)
                .FirstOrDefaultAsync() ?? throw new DataNotFoundException();

        private void ValidatePersonalNumber(string personalNumber)
        {
            if (!Regex.IsMatch(personalNumber, @"^\d{11}$"))
                throw new DataValidationException("Wprowadzono nieprawidłowy numer PESEL");
        }
        #endregion
    }
}
