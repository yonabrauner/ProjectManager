using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProjectManager.Api.Services.Interfaces;
using ProjectManager.Api.DTOs;


namespace ProjectManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var response = await _authService.RegisterAsync(dto);
                if (!response.Success)
                    return BadRequest(new { error = response.Message });

                // 201 Created is more RESTful than 200 OK
                return StatusCode(StatusCodes.Status201Created, response.Data);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var response = await _authService.LoginAsync(dto);
                if (!response.Success)
                    return Unauthorized(new { error = response.Message });

                return Ok(response.Data); // JWT token
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("validate")]
        [Authorize]
        public IActionResult Validate()
        {
            return Ok(new { message = "Token is valid" });
        }
    }
}