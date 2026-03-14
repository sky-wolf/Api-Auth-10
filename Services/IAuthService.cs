using Api.Dtos;

namespace Api.Services
{
    public interface IAuthService
    {
        public Task<ResponseDto> AddAsync(CreateDto dto);
        public Task<bool> LoginAsync(LoginDto loginDto);
    }
}
