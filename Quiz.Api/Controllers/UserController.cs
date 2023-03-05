using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Quiz.Api.Filters;
using Quiz.Data.Helpers;
using Quiz.Infrastructure.Interfaces;
using Quiz.Shared.DTOs;

namespace Quiz.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        #region Private fields
        private readonly IDataService _dataService;
        #endregion

        #region Constructor
        public UserController(IDataService dataService)
        {
            _dataService = dataService;
        }
        #endregion


        #region Actions

        [HttpPost("token")]
        public IActionResult GetToken([FromBody] TokenDto content)
        {
            var rsaKey = RSA.Create();
            rsaKey.ImportRSAPrivateKey(content?.PrivateKey, out _);

            //handler do tworzenie i walidacji tokenów
            var handler = new JsonWebTokenHandler();

            var key = new RsaSecurityKey(rsaKey);

            var token = handler.CreateToken(new SecurityTokenDescriptor()
            {
                Issuer = "https://localhost:7011",
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("sub", Guid.NewGuid().ToString()),
                    new Claim("name", "Filip")
                }),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256),
            });

            return Ok(token);
        }

        [HttpGet("auth")]
        public IActionResult TryAuthenticate([FromQuery] string token)
        {
            return Ok(User.FindFirst("sub")?.Value ?? "empty");
        }

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

        //[ActiveUser]
        //[Authorize] - też działa
        [HttpGet("data")]
        public IActionResult GetData()
        {
            var fromCookie = User.FindFirst("user")?.Value ?? "empty";
            return Ok(fromCookie);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] int userId)
        {
            var user = await _dataService.GetUserById(userId);
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

