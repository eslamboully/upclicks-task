using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class UserRegisterDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(4)]
        public string Password { get; set; }
    }
}