using AutoMapper;
using EventStock.Application.Mapping;
using EventStock.Domain.Models;

namespace EventStock.Application.Dto.Stock
{
    public class ViewStockDto : IMapFrom<EventStock.Domain.Models.Stock>
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public Address? Address { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EventStock.Domain.Models.Stock, ViewStockDto>();
        }
    }
}