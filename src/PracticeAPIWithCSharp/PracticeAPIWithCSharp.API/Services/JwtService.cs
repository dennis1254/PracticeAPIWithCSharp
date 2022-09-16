using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PracticeAPIWithCSharp.API.Interfaces;
using PracticeAPIWithCSharp.API.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace PracticeAPIWithCSharp.API.Services
{
    public class JwtService : IJwt
    {
        Dictionary<string, string> UsersRecords = new Dictionary<string, string>
        {
            { "user1","password1"},
            { "user2","password2"},
            { "user3","password3"},
        };

        private readonly IConfiguration _config;
        public JwtService(IConfiguration config)
        {
            _config = config;
        }
        public Response<Tokens> GenerateToken(User user)
        {
            if (!UsersRecords.Any(x => x.Key == user.Name && x.Value == user.Password))
            {
                return new Response<Tokens> { Message = "Invalid user credentials"};
            }

            // Else we generate JSON Web Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_config["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                  new Claim(ClaimTypes.Name, user.Name)
                }),
                Expires = DateTime.UtcNow.AddSeconds(_config.GetValue<int>("JWT:ExpirationInSeconds")),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new Response<Tokens>
            { 
                Message = "Successful",
                Success=true,
                Data = new Tokens { Token = tokenHandler.WriteToken(token) }
            };
        }
    }
}
