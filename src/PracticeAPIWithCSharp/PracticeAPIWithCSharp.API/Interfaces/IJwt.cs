using PracticeAPIWithCSharp.API.Models;
using System.Security.Claims;

namespace PracticeAPIWithCSharp.API.Interfaces
{
    public interface IJwt
    {
        Response<Tokens> GenerateToken(User user);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);
    }
}
