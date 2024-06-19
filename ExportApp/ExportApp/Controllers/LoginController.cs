using ExportApp.Models;
using ExportApp.Services.Employee;
using ExportApp.Services.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExportApp.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ILoginService _loginService;

        public LoginController(ILogger<LoginController> logger, ILoginService loginService)
        {
            _logger = logger;
            _loginService = loginService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("VerifyEmployeeCredentials")]
        public IActionResult VerifyEmployeeCredentials(CredentailsInfo credentailsInfo)
        {
            var loginInfo = _loginService.VerifyCredentialsAndGetJWTToken(credentailsInfo);
            if (loginInfo == null)
            {
                return BadRequest("Invalid Credentials");
            }
            return Ok(loginInfo);
        }
    }
}
