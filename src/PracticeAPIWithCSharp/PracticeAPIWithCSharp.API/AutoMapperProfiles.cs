using AutoMapper;
using PracticeAPIWithCSharp.API.Models;
using PracticeAPIWithCSharp.API.ViewModels;

namespace PracticeAPIWithCSharp
{
    public class AutoMapperProfiles: Profile
    {
            public AutoMapperProfiles()
            {
                CreateMap<User, LoginModel>().ReverseMap().ForMember(dest => dest.UserName, act => act.MapFrom(src => src.UserName));// To map properties with different names
                CreateMap<Tokens, RefreshModel>().ReverseMap();
               
            }
    }
}
