using Seatsure.Domain;

namespace Seatsure.Application.Repositories;

public interface IEventRepository
{
    Task<Event?> GetByIdAsync(Guid id);
    Task<(IEnumerable<Event> Items, int TotalCount)> GetPublishedAsync(int page, int pageSize);
    Task AddAsync(Event ev);
}
