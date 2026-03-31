using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Service;


namespace WebApplication1.Controller
{
    [ApiController]
    [Route("api/[controller]")] // https://localhost:7190/api/auth
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }



        //REST API
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _authService.Authenticate(request);
                if (response == null)
                    return Unauthorized("Invalid token or password");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("register")] // https://localhost:7190/api/auth/register (POST)
        public async Task<ActionResult<string>> Register([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _authService.Register(request);
                if (response == null)
                    return Unauthorized("Invalid token or password");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
