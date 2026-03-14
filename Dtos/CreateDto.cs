using System.ComponentModel.DataAnnotations;

namespace Api.Dtos
{
    public class CreateDto
    {
        public bool? Organisator { get; set; }
        public string? Name { get; set; }

        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }

    }
}
