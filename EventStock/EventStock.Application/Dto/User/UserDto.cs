using AutoMapper;
using EventStock.Application.Mapping;

namespace EventStock.Application.Dto.User
{
    public class UserDto : IMapFrom<EventStock.Domain.Models.User>
    {
        public string? Id { get; set; } 
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public List<string> Roles { get; set; } = new List<string>();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EventStock.Domain.Models.User, UserDto>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore());
        }
    }
}
