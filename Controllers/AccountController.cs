using Api.Dtos;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetOne()
        {
            return Ok("Hello from AccountController!");
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok("Hello from AccountController Getall!");
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] Dtos.CreateDto dto)
        {
            var result = await _authService.AddAsync(dto);
            return Ok(result);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ResponseDto> Login([FromBody] LoginDto login)
        {
            await _authService.LoginAsync(login);

            return new()
            {
                Success = true,
                Message = "Login successful"
            };
        }

        [HttpGet("profile")]
        [Authorize(Roles = "organisator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirst(ClaimTypes.Email)?.Value;
            
            var profile = await _authService.FindByEmail(userId);

            return Ok(profile);
        }
    
    [HttpGet("users")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListUsers()
        {
            var profile = await _authService.GetAllAsync();
            return Ok(profile);
        }

        [HttpPost("token/refresh")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Refresh([FromBody] TokenDto model)
        {
            _Logger.LogInformation("Refresh called");
            var exist = await _authService.RefrechTokenAsync(model);

            if (exist.Status == ReturnCode.Unauthorized) return Unauthorized();

            return Ok(exist);

        }

        [HttpPost("forgotpassword")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ResponseDto> ForgotPassword(EmailDto email)
        {
            try
            {
                _Logger.LogInformation("Forgot Password");
                if (!String.IsNullOrEmpty(email.ToString()))
                {
                    _Logger.LogInformation("Forgot Password");
                    var exist = await _authService.FindByEmail(email);

                    if (exist != null)
                    {
                        exist.PasswordResetToken = _tokenGenerator.GenerateToken();
                        exist.ResetTokenExpiresAt = DateTime.Now.AddDays(3);
                        _context.ApplicationUsers.Update(exist);
                        await _context.SaveChangesAsync();
                    }
                }
                return new()
                {
                    IsSuccess = true,
                    Result = null,
                    Message = "Forgot Password"
                };
            }
            catch
            {
                _Logger.LogInformation("Forgot Password");
                return new ResponseDto()
                {
                    IsSuccess = false,
                    Message = "Forgot Password",
                    Result = null,
                };
            }
        }
    }
}
