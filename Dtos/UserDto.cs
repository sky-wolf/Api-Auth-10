using System.ComponentModel.DataAnnotations;

namespace Api.Dtos
{
    public class UserDto
    {

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [MinLength(5)]
        [MaxLength(100)]
        public string? Name { get; set; }

        public string? Role { get; set; }

    }
}
