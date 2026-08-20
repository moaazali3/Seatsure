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
        return await _context.Events.FindAsync(id);
    }

    public async Task<(IEnumerable<Event> Items, int TotalCount)> GetPublishedAsync(int page, int pageSize)
    {
        var query = _context.Events
            .Include(e => e.TicketTypes)
            .Where(e => e.Status == EventStatus.Published);

        var count = await query.CountAsync();
        var items = await query
            .OrderBy(e => e.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, count);
    }

    public async Task AddAsync(Event ev)
    {
        await _context.Events.AddAsync(ev);
    }
}
