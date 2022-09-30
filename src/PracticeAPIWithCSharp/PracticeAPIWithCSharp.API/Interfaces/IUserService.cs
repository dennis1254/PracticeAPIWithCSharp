using PracticeAPIWithCSharp.API.Models;
using PracticeAPIWithCSharp.API.ViewModels;

namespace PracticeAPIWithCSharp.API.Interfaces
{
    public interface IUserService
    {
        Response<Tokens> Authenticate(LoginModel user);
        Response<Tokens> RefreshToken(RefreshModel tokens);
    }
}
