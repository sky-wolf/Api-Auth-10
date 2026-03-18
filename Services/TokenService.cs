using Api.Data;
using Api.Helpers;
using Api.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Api.Services
{
    public class TokenService : ITokenService
    {
        private readonly APIDbContext _context;
        private readonly jwtOptions _jwtOptions;
        public TokenService(APIDbContext context, jwtOptions jwtOptions)
        {
            _context = context;
            _jwtOptions = jwtOptions;
        }

        public string GenerateAccessToken(ApplicationUser user, List<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var key = System.Text.Encoding.UTF8.GetBytes(_jwtOptions.Key);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature);

            var claimsList = new List<System.Security.Claims.Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Role, string.Join(",", roles))
            };

            var tokenDescriptor = new JwtSecurityToken
                ( 
                    issuer: _jwtOptions.Issuer,
                    audience: _jwtOptions.Audience,
                    claims: claimsList,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: credentials 
                );

            return tokenHandler.WriteToken(tokenDescriptor);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
                rng.GetBytes(randomNumber);

                return Convert.ToBase64String(randomNumber);
            }
        }

        public string Token()
        {
            throw new NotImplementedException();
        }
    }
}
