using Api.Dtos;
using Api.Models;

namespace Api.Services
{
    public interface IAuthService
    {
        public Task<ResponseDto> AddAsync(CreateDto dto);
        public Task<ApplicationUser> FindByEmail(string email);
        public Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
    }
}
