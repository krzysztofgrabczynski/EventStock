using AutoMapper;
using EventStock.Application.Mapping;
using EventStock.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace EventStock.Application.Dto.Stock
{
    public class CreateStockDto : IMapFrom<Domain.Models.Stock>
    {
        [Required]
        public string Name { get; set; }
        public Address? Address{ get; set; }
        public ICollection<Domain.Models.User> Users { get; set; } = new List<Domain.Models.User>();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateStockDto, Domain.Models.Stock>();
        }
    }
}