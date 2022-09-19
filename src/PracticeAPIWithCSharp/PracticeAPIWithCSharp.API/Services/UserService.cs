using PracticeAPIWithCSharp.API.Interfaces;
using PracticeAPIWithCSharp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PracticeAPIWithCSharp.API.Services
{
    public class UserService:IUserService
    {
      
        private readonly IJwt _jwt;

        public UserService(IJwt jwt)
        {
            _jwt = jwt;
        }

        public UserService()
        {
                
        }
        public Response<Tokens> Authenticate (User user)
        {
            if (!Utility.UsersRecords.Any(x => x.UserName == user.UserName && x.Password == user.Password))
            {
                return new Response<Tokens> { Message = "Invalid user credentials" };
            }
            var resp = _jwt.GenerateToken(user);
            user = Utility.UsersRecords.FirstOrDefault(x => x.UserName == user.UserName);
            user.RefreshToken = resp.Data.RefreshToken;
            user.IsActive = true;
            return resp;

        }
        public Response<Tokens> RefreshToken(Tokens tokens)
        {
            try
            {
                var principal = _jwt.GetPrincipalFromExpiredToken(tokens.AccessToken);
                var username = principal.Identity?.Name;
                if (!Utility.UsersRecords.Any(x => x.UserName == username && x.RefreshToken == tokens.RefreshToken))
                {
                    return new Response<Tokens> { Message = "Invalid token credentials" };
                }
                var resp = _jwt.GenerateToken(new User { UserName = username});
                var user = Utility.UsersRecords.FirstOrDefault(x => x.UserName == username);
                user.RefreshToken = resp.Data.RefreshToken;
                user.IsActive = true;
                return resp;

            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
