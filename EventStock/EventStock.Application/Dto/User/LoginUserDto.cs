using System.ComponentModel.DataAnnotations;

namespace EventStock.Application.Dto.User
{
    public class LoginUserDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
