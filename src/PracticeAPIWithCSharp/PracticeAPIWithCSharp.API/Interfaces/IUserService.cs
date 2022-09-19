using PracticeAPIWithCSharp.API.Models;

namespace PracticeAPIWithCSharp.API.Interfaces
{
    public interface IUserService
    {
        Response<Tokens> Authenticate(User user);
        Response<Tokens> RefreshToken(Tokens tokens);
    }
}
