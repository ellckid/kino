using kino.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace kino.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IOptions<AuthOptions> authOptions;

        public AuthController(IOptions<AuthOptions> authOptions)
        {
            this.authOptions = authOptions;
        }
        private List<Account> Accounts => new List<Account>
        {
            new Account()
            {
                Id = Guid.Parse("1"),
                Email = "user@mail.ru",
                Password = "user123",
                Roles = new Role[] {Role.User}
            },
            new Account()
            {
                Id = Guid.Parse("2"),
                Email = "user2@mail.ru",
                Password = "user321",
                Roles = new Role[] {Role.User}
            },
            new Account()
            {
                Id = Guid.Parse("3"),
                Email = "admin@mail.ru",
                Password = "admin123",
                Roles = new Role[] {Role.Admin}
            }
        };

        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody]Login request)
        {
            var user = AuthenticateUser(request.Email, request.Password); // идентифицируем пользователя

            if (User != null)
            {
                var token = GenerateJWT(user);

                return Ok(new
                {
                    access_token = token
                });
            }
            return Unauthorized();
        }
        private Account AuthenticateUser (string email, string password)
        {
            return Accounts.SingleOrDefault(u => u.Email == email && u.Password == password);
        }

        private string GenerateJWT(Account user)
        {
            var AuthParams = authOptions.Value;

            var secutityKey = AuthParams.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(secutityKey, SecurityAlgorithms.HmacSha256);

            var Claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

            foreach (var role in user.Roles)
            {
                Claims.Add(new Claim("role", role.ToString()));
            }

            var token = new JwtSecurityToken(AuthParams.Issuer, AuthParams.Audience,
                Claims,
                expires: DateTime.Now.AddSeconds(AuthParams.TokenLifeTime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
