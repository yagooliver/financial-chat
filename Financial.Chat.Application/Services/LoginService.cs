using Financial.Chat.Domain.Core.Entity;
using Financial.Chat.Domain.Core.Interfaces;
using Financial.Chat.Domain.Core.Interfaces.Services;
using Financial.Chat.Domain.Shared.Entity;
using Financial.Chat.Domain.Shared.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Financial.Chat.Application.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public LoginService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        public User Authenticate(string email, string password)
        {
            var passwordEncrypt = Cryptography.PasswordEncrypt(password);
            return _userRepository.GetByExpression(x => x.Email == email && x.Password == passwordEncrypt).FirstOrDefault();
        }

        public TokenJWT GetToken(Guid id, string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                    new Claim(ClaimTypes.Email, email)
                };
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: credentials);

            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenJWT(true, encodetoken);
        }
    }
}
