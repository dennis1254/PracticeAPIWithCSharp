using PracticeAPIWithCSharp.API.Models;

namespace PracticeAPIWithCSharp.API.Interfaces
{
    public interface IJwt
    {
        Response<Tokens> GenerateToken(User user);
    }
}
