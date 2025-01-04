using System.ComponentModel.DataAnnotations;

namespace EventStock.Application.Dto.User
{
    public class CreateUserDto
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Passwords must be the same!")]
        public string ConfirmPassword { get; set; }
    }
}