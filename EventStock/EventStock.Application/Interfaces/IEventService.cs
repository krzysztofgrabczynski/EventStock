using EventStock.Application.Dto.Event;
using EventStock.Domain.Models;

namespace EventStock.Application.Interfaces
{
    public interface IEventService
    {
        Task<int> CreateEventAsync(EventDto Event);
        Task<ViewEventDto> GetEventAsync(int id);
        Task<ViewEventDto> UpdateEventAsync(EventDto eventDto);
        Task DeleteEventAsync(int id);

        Task AddUserAsync(User user);
        Task<List<EventUserDto>> ListUsersByEventIdAsync(int id);

        Task ChangeEventStatus(EventStatus status);
    }
}
