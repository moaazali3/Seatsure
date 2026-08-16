using Microsoft.EntityFrameworkCore;
using Seatsure.Application.Repositories;
using Seatsure.Domain;

namespace Seatsure.Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly AppDbContext _context;

    public EventRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Event?> GetByIdAsync(Guid id)
    {
        // TODO: Implement GetByIdAsync (including TicketTypes) using DbContext
        throw new NotImplementedException();
    }

    public async Task<(IEnumerable<Event> Items, int TotalCount)> GetPublishedAsync(int page, int pageSize)
    {
        // TODO: Implement GetPublishedAsync with pagination and ordering using DbContext
        throw new NotImplementedException();
    }

    public async Task AddAsync(Event ev)
    {
        // TODO: Implement AddAsync using DbContext
        throw new NotImplementedException();
    }

    public async Task SaveChangesAsync()
    {
        // TODO: Implement SaveChangesAsync using DbContext
        throw new NotImplementedException();
    }
}
