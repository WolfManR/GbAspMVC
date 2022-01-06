using IdentityServer.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthenticationService _authenticationService;

        public AuthController(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromQuery] string user, string password)
        {
            string token = await _authenticationService.Authenticate(user, password);
            if (token is null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("registeruser")]
        public async Task<IActionResult> RegisterUser([FromQuery] string user, string password)
        {
            var succeed = await _authenticationService.RegisterUser(user, password);
            return succeed ? Ok() : BadRequest();
        }
    }
}
