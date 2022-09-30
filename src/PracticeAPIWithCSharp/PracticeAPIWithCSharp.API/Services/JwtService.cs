using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PracticeAPIWithCSharp.API.Interfaces;
using PracticeAPIWithCSharp.API.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PracticeAPIWithCSharp.API.Services
{
    public class JwtService : IJwt
    {
       

        private readonly IConfiguration _config;
        public JwtService(IConfiguration config)
        {
            _config = config;
        }
        public Response<Tokens> GenerateToken(User user)
        {
            
            // Else we generate JSON Web Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_config["JWT:Key"]);
			var expiresAt = DateTime.UtcNow.AddSeconds(_config.GetValue<int>("JWT:ExpirationInSeconds"));

			var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                  new Claim(ClaimTypes.Name, user.UserName)
                }),
				Issuer = _config["JWT:Issuer"],
				Audience = _config["JWT:Audience"],
				Expires = expiresAt,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
			var refreshToken = GenerateRefreshToken();			

			return new Response<Tokens>
            { 
                Message = "Successful",
                Success=true,
                Data = new Tokens { AccessToken = tokenHandler.WriteToken(token),RefreshToken =  refreshToken, ExpiresAt = expiresAt}
            };
        }
		

		public string GenerateRefreshToken()
		{
			var randomNumber = new byte[32];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randomNumber);
				return Convert.ToBase64String(randomNumber);
			}
		}

		public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
		{
			var Key = Encoding.UTF8.GetBytes(_config["JWT:Key"]);

			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = false,
				ValidateAudience = false,
				ValidateLifetime = false,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Key),
				ClockSkew = TimeSpan.Zero
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
			JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
			if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
			{
				throw new SecurityTokenException("Invalid token");
			}


			return principal;
		}
	}
}
