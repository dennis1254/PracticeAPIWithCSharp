using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticeAPIWithCSharp.API.Interfaces;
using PracticeAPIWithCSharp.API.Models;

namespace PracticeAPIWithCSharp.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwt _jwt;
        public AuthController(IJwt jwt)
        {
            _jwt = jwt;
        }
        [HttpPost]
        public ActionResult<Response<Tokens>> Authenticate(User user)
        {
            var resp = _jwt.GenerateToken(user);
            return Ok(resp);
        }
    }
}
