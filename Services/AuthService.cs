using Api.Data;
using Api.Dtos;
using Api.Models;
using System.Security.Cryptography;
using System.Text;

namespace Api.Services
{
    public class AuthService/*(APIDbContext context) */: IAuthService
    {
        private readonly APIDbContext _context;

        public AuthService(APIDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseDto> AddAsync(CreateDto dto)
        {
            try
            {
                CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);

                ApplicationUser user = new ApplicationUser()
                {
                    Email = dto.Email.ToLower(),
                    Name = dto.Name,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };

                var result = await _context.ApplicationUsers.AddAsync(user);
                await _context.SaveChangesAsync();



                return new ResponseDto()
                {
                    ResultData = user,
                    Success = true,
                    Message = "User created successfully"
                };
            }
            catch (Exception e)
            {
                return new ResponseDto()
                {
                    Success = false,
                    Message = e.Message
                };
            }
        }

        public async Task<bool> LoginAsync(LoginDto? loginDto)
        {
            //throw new NotImplementedException();

            ApplicationUser exist = await _context.ApplicationUsers.FindAsync(loginDto.Email.ToLower());

            if (exist != null) 
            {
                if(VerifyPassword(loginDto.Password, exist.PasswordHash!, exist.PasswordSalt!))
                {
                    List<string> roles = new List<string>();

                    var getUserRoles = await _context.UserRoles.FindAsync(exist.Id);

                    //foreach (var role in getUserRoles) {

                }

                return true;
            }
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (HMACSHA512 hmac = new HMACSHA512(passwordHash))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
                return true;
            }
        }
    }
}
