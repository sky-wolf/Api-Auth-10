using Api.Data;
using Api.Dtos;
using Api.Models;

namespace Api.Services
{
    public interface ITokenService
    {
       
        public LoginResponseDto Token(ApplicationUser user);
        public string GenerateAccessToken(ApplicationUser user);
        public string GenerateRefreshToken();
    }
}
