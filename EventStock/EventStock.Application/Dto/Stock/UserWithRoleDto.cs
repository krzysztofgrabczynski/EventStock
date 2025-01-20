using AutoMapper;
using EventStock.Application.Mapping;
using System.ComponentModel.DataAnnotations;

namespace EventStock.Application.Dto.Stock
{
    public class UserWithRoleDto : IMapFrom<EventStock.Domain.Models.User>
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public int Role { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EventStock.Domain.Models.User, UserWithRoleDto>()
                .ForMember(u => u.Role, opt => opt.Ignore());
        }
    }
}
