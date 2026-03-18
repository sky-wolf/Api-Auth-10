using Api.Data;
using Api.Models;

namespace Api.Services
{
    public interface ITokenService
    {
       
        public string Token();
        public string GenerateAccessToken(ApplicationUser user, List<string> roles);
        public string GenerateRefreshToken();
    }
}
