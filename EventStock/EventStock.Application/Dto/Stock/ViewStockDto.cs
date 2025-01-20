using AutoMapper;
using EventStock.Application.Mapping;
using EventStock.Domain.Models;

namespace EventStock.Application.Dto.Stock
{
    public class ViewStockDto : IMapFrom<EventStock.Domain.Models.Stock>
    {
        public required string Name { get; set; }
        public Address? Address { get; set; }
        public ICollection<UserWithRoleDto> Users { get; set; } = new List<UserWithRoleDto>();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EventStock.Domain.Models.Stock, ViewStockDto>();
        }
    }
}