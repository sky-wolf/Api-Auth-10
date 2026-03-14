using Api.Dtos;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Dtos.CreateDto dto)
        {
            var result = await _authService.AddAsync(dto);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            await _authService.LoginAsync(login);

                return Ok();
        }

    }
}
