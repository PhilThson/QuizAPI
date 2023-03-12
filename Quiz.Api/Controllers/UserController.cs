using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Quiz.Api.Filters;
using Quiz.Data.Helpers;
using Quiz.Infrastructure.Interfaces;
using Quiz.Infrastructure.Logging;
using Quiz.Shared.DTOs;

namespace Quiz.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        #region Private fields
        private readonly IDataService _dataService;
        private readonly ILogger<UserController> _logger;
        #endregion

        #region Constructor
        public UserController(IDataService dataService, 
            ILogger<UserController> logger)
        {
            _dataService = dataService;
            _logger = logger;
        }
        #endregion

        #region Actions

        [HttpGet("login")]
        public IActionResult Login([FromQuery] string returnUrl = "/")
        {
            //returnUrl - strona/endpoint z ktorego nastapilo przekierowanie
            //Po zalogowaniu można ew. przekierować na stronę z której
            //użytkownik 'przyszedł'
            return Unauthorized("Proszę się zalogować");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] SimpleUserDto user)
        {
            var userFromDb = await _dataService.GetUserByEmail(user.Email) ??
                throw new DataNotFoundException(
                    "Nie znaleziono uzytkownika o podanym adresie email");

            if (!SecurePasswordHasher.Verify(user.Password, userFromDb.PasswordHash))
                throw new DataValidationException("Wprowadzono niepoprawne hasło");

            var principal = CreateClaimsPrincipal(user.Email);

            await HttpContext
                .SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                principal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                });

            _logger.Info($"Zalogowano użytkownika '{user.Email}'");
            return Ok("Zalogowano");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            _ = User.FindFirst("user")?.Value ??
                throw new DataNotFoundException(
                    "Nie znaleziono zalogowanego uzytkownika");

            await HttpContext
                .SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return NoContent();
        }

        [ActiveUser]
        [HttpGet("data")]
        public IActionResult GetData()
        {
            var fromCookie = User.FindFirst("user")?.Value ?? "empty";
            return Ok(fromCookie);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] int id)
        {
            var user = await _dataService.GetUserById(id);
            return Ok(user);
        }

        [HttpGet("byEmail")]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
        {
            var user = await _dataService.GetUserByEmail(email);
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] CreateUserDto userDto)
        {
            var user = await _dataService.AddUser(userDto);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        #region Role
        [HttpGet("roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _dataService.GetAllRoles();
            return Ok(roles);
        }
        #endregion

        #endregion

        #region Private methods

        private ClaimsPrincipal CreateClaimsPrincipal(string userEmail) =>
            new ClaimsPrincipal(
                new ClaimsIdentity(
                    new List<Claim>
                    {
                        new Claim("user", userEmail)
                    },
                    CookieAuthenticationDefaults.AuthenticationScheme
                )
            );

        #endregion
    }
}

