using Api.Data;
using Api.Dtos;
using Api.Helpers;
using Api.Models;
using Microsoft.Extensions.Options;
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
        public TokenService(APIDbContext context, IOptions<jwtOptions> jwtOptions)
        {
            _context = context;
            _jwtOptions = jwtOptions.Value;
        }

        public string GenerateAccessToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = System.Text.Encoding.UTF8.GetBytes(_jwtOptions.Key);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature);

            var getUserRoles = _context.UserRoles.Where(ur => ur.UserId == user.Id).ToList();

            var claimsList = new List<Claim>
            {
                //new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                //new Claim(ClaimTypes.Name, user.Name ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Role, string.Join(",", getUserRoles))
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

        public LoginResponseDto Token(ApplicationUser user)
        {
            return new LoginResponseDto
            {
                AccessToken = GenerateAccessToken(user),
                RefreshToken = GenerateRefreshToken()
            };
        }
    }
}
