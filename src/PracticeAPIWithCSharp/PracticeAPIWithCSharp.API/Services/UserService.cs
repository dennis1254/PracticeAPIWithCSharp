using AutoMapper;
using PracticeAPIWithCSharp.API.Interfaces;
using PracticeAPIWithCSharp.API.Models;
using PracticeAPIWithCSharp.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PracticeAPIWithCSharp.API.Services
{
    public class UserService:IUserService
    {
      
        private readonly IJwt _jwt;
        private readonly IMapper _mapper;

        public UserService(IJwt jwt, IMapper mapper)
        {
            _jwt = jwt;
            _mapper = mapper;
        }

        public UserService()
        {
                
        }
        public Response<Tokens> Authenticate (LoginModel model)
        {
            if (!Utility.UsersRecords.Any(x => x.UserName == model.UserName && x.Password == model.Password))
            {
                return new Response<Tokens> { Message = "Invalid user credentials" };
            }
            var user = _mapper.Map<User>(model);
            var resp = _jwt.GenerateToken(user);
            user = Utility.UsersRecords.FirstOrDefault(x => x.UserName == model.UserName);
            user.RefreshToken = resp.Data.RefreshToken;
            user.IsActive = true;
            return resp;

        }
        public Response<Tokens> RefreshToken(RefreshModel tokens)
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
