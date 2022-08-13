using Microsoft.AspNetCore.Mvc;
using StalkerMUD.Common.Models;
using StalkerMUD.Server.Models;
using StalkerMUD.Server.Services;

namespace StalkerMUD.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("Register")]
        public Task<AuthenticateResponse> Register([FromBody] AuthenticateRequest request)
        {
            return _authService.RegisterAsync(request);
        }

        [HttpPost]
        [Route("Login")]
        public Task<AuthenticateResponse> Login([FromBody] AuthenticateRequest request)
        {
            return _authService.LoginAsync(request);
        }
    }
}
