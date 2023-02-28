using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Quiz.Data.Helpers;
using Quiz.Infrastructure.Interfaces;

namespace Quiz.Api.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IDataService _dataService;
        private static readonly string AuthScheme = "cookie";

        public UserController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost("token")]
        public IActionResult GetToken([FromBody] Token content)
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
        public IActionResult Login([FromQuery] string returnUrl)
        {
            //returnUrl - strona/endpoint z ktorego nastapilo przekierowanie
            return Unauthorized("Please log in");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var userFromDb = await _dataService.GetUserByEmail(user.Email) ??
                throw new DataNotFoundException(
                    "Nie znaleziono uzytkownika o podanym adresie email");

            if (userFromDb.PasswordHash != user.PasswordHash)
                throw new DataValidationException("Wprowadzono niepoprawne hasło");

            var principal = CreateClaimsPrincipal(user.Email);

            await HttpContext.SignInAsync(AuthScheme, principal,
                new AuthenticationProperties
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

            await HttpContext.SignOutAsync(AuthScheme);
            return NoContent();
        }

        [Authorize]
        [HttpGet("data")]
        public IActionResult GetData()
        {
            var fromCookie = User.FindFirst("user")?.Value ?? "empty";
            return Ok(fromCookie);
        }

        private ClaimsPrincipal CreateClaimsPrincipal(string userEmail) =>
            new ClaimsPrincipal(
                new ClaimsIdentity(
                    new List<Claim>
                    {
                        new Claim("user", userEmail)
                    },
                    AuthScheme
                )
            );
    }

    public class Token
    {
        public byte[] PrivateKey { get; set; }
    }

    public class User
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}

