using AutoMapper;
using EventStock.Application.Mapping;
using System.ComponentModel.DataAnnotations;

namespace EventStock.Application.Dto.User
{
    public class CreateUserDto : IMapFrom<EventStock.Domain.Models.User>
    {
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

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateUserDto, EventStock.Domain.Models.User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(src => src.Email));
        }
    }
}