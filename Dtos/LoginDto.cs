using System.ComponentModel.DataAnnotations;

namespace Api.Dtos
{
    public class LoginDto
    {
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
