using Seatsure.Application.DTOs.Common;
using Seatsure.Application.DTOs.Events;

namespace Seatsure.Application.Services.Interfaces;

public interface IEventService
{
    Task<PagedResult<EventDto>> GetPublishedAsync(int page, int pageSize);
    Task<EventDetailDto> GetByIdAsync(Guid id);
    Task<EventDto> CreateAsync(Guid organizerId, CreateEventRequest request);
    Task<EventDto> PublishAsync(Guid eventId, Guid organizerId);
}
