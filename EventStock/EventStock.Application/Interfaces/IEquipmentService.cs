using EventStock.Application.Dto.Equipment;

namespace EventStock.Application.Interfaces
{
    public interface IEquipmentService
    {
        Task<int> CreateEquipmentAsyng(EquipmentDto equipment);
        Task<ViewEquipmentDto> GetEquipmentAsync(int id);
        Task<ViewEquipmentDto> UpdateEquipmentAsync(EquipmentDto equipment);
        Task DeleteEquipmentAsync(int id);
    }
}
