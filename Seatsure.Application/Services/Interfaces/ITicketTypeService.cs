using Seatsure.Application.DTOs.TicketTypes;

namespace Seatsure.Application.Services.Interfaces;

public interface ITicketTypeService
{
    Task<IEnumerable<TicketTypeDto>> GetByEventIdAsync(Guid eventId);
    Task<TicketTypeDto> AddAsync(Guid eventId, Guid organizerId, CreateTicketTypeRequest request);
}
