using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticeAPIWithCSharp.API.Interfaces;
using PracticeAPIWithCSharp.API.Models;
using System.Collections.Generic;
using System.Management;

namespace PracticeAPIWithCSharp.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _service;
        public AuthController(IUserService service)
        {
            _service = service;
        }
        [HttpPost]
        public ActionResult<Response<Tokens>> Authenticate(User user)
        {
            var resp = _service.Authenticate(user);
            var userAgent = Request.Headers["user-agent"];
            var forwarded = Request.Headers["x-fowarded-for"];
            var xforwarded =Request.Headers["HTTP_X_FORWARDED_FOR"];
             var em = Request.Headers["X-em-uid"];
            var accept = Request.Headers["accept"];
            var jphone = Request.Headers["x-jphone-uid"];
            var subno = Request.Headers["x-up-subno"];
            var dcmguid = Request.Headers["x-dcmguid"];
            var ipAddress = HttpContext.Connection.RemoteIpAddress;
            
            return Ok(resp);
        }

        [HttpPost]
        public ActionResult<Response<Tokens>> Refresh(Tokens tokens)
        {
            var resp = _service.RefreshToken(tokens);

            return Ok(resp);
        }
    }
}
