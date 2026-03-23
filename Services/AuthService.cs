using Api.Data;
using Api.Dtos;
using Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Api.Services
{
    public class AuthService/*(APIDbContext context) */: IAuthService
    {
        private readonly APIDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthService(APIDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<ResponseDto> AddAsync(CreateDto? dto)
        {
            try
            {
                CreatePasswordHash(dto!.Password!, out byte[] passwordHash, out byte[] passwordSalt);

                ApplicationUser user = new ApplicationUser()
                {
                    Email = dto.Email!.ToLower(),
                    Name = dto.Name,
                };

                var result = await _context.ApplicationUsers.AddAsync(user);
                await _context.SaveChangesAsync();

                Hash hash = new Hash()
                {
                    UserId = result.Entity.Id,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };

                await _context.Hashes.AddAsync(hash);
                await _context.SaveChangesAsync();

                //Roles

                string Orgisation = (bool)dto.Organisatör! ? "organisatör" : "user";
                //.Include(SystemRoles => SystemRoles.RoleId)
                var roleCheck = await _context.SystemRoles.FirstOrDefaultAsync(r => r.Name == Orgisation);
                
                if (roleCheck != null) 
                {
                    
                    await _context.UserRoles.AddAsync(new UserRole()
                    {
                        UserId = result.Entity.Id,
                        RoleId = roleCheck.Id!.Value,
                    });
                    await _context.SaveChangesAsync();
                }


                return new ResponseDto()
                {
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

        public async Task<ApplicationUser> FindByEmail(string email)
        {
            try
            {
                var exist = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == email.ToString());


                return exist!;

            }
            catch
            {
                return new ApplicationUser();

            }


        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto? loginDto)
        {
            var result = new LoginResponseDto();

            ApplicationUser? exist = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == loginDto!.Email);
            Hash? hash = await _context.Hashes.FirstOrDefaultAsync(h => h.UserId == exist!.Id);

            if (exist != null) 
            {
                if(VerifyPassword(loginDto?.Password!, hash!.PasswordHash!, hash.PasswordSalt!))
                {
                    List<string> roles = new List<string>();

                    var getUserRoles = await _context?.UserRoles.Where(r => r.UserId == exist.Id).ToListAsync();
                    
                     if (getUserRoles != null)
                     {
                        foreach (var role in getUserRoles)
                        {
                            //net to ficks 
                            var roleName = await _context.SystemRoles.Where(r => r.Id == role.RoleId);
                            if (roleName != null)
                            {
                                roles.Add(roleName.Kode!);
                            }
                        }
                     }
                    
                      result = _tokenService.Token(exist);
                      

                    return result;
                }
                return null;
            }
            return null;
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
            using (HMACSHA512 hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.
                    ComputeHash(Encoding.UTF8.GetBytes(password));
                
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
