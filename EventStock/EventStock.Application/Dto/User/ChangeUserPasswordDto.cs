using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventStock.Application.Dto.User
{
    public class ChangeUserPasswordDto
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Passwords must be the same!")]
        public string ConfirmPassword { get; set; }

    }
}
