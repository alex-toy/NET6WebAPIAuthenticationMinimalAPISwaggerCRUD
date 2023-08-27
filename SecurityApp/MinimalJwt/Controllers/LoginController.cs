using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MinimalJwt.Models;
using MinimalJwt.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MinimalJwt.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public LoginController(IUserService userService, IConfiguration config)
        {
            _userService = userService;
            _configuration = config;
        }

        [HttpPost("Login")]
        public IResult Login([FromBody] UserLogin user)
        {
            bool credentialsOK = string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password);
            if (!credentialsOK) Results.BadRequest("Invalid user credentials"); ;

            User loggedInUser = _userService.Get(user);
            if (loggedInUser is null) return Results.NotFound("User not found");

            Claim[] claims = GetClaims(loggedInUser);
            string tokenString = GenerateToken(claims);

            return Results.Ok(tokenString);
        }

        private Claim[] GetClaims(User loggedInUser)
        {
            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, loggedInUser.Username),
                new Claim(ClaimTypes.Email, loggedInUser.EmailAddress),
                new Claim(ClaimTypes.GivenName, loggedInUser.GivenName),
                new Claim(ClaimTypes.Surname, loggedInUser.Surname),
                new Claim(ClaimTypes.Role, loggedInUser.Role)
            };

            return claims;
        }

        private string GenerateToken(Claim[] claims)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key")));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: _configuration.GetValue<string>("Jwt:Issuer"),
                audience: _configuration.GetValue<string>("Jwt:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(60),
                notBefore: DateTime.UtcNow,
                signingCredentials: signingCredentials
            );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }
    }
}
