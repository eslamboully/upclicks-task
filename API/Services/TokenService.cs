using API.Interfaces;
using System.Threading.Tasks;
using API.Entities;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Security.Claims;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));    
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);    
    
            var claims = new List<Claim>{
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            // var token = new JwtSecurityToken(_config["Jwt:Issuer"],    
            //   _config["Jwt:Issuer"],    
            var token = new JwtSecurityToken(null,
              null,
              claims,    
              expires: DateTime.Now.AddDays(30),    
              signingCredentials: credentials);    
    
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}