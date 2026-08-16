using Microsoft.EntityFrameworkCore;
using Seatsure.Application.Repositories;
using Seatsure.Domain;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        var result = await _context.Events.FindAsync(id);
        return result;
    }

    public async Task<(IEnumerable<Event> Items, int TotalCount)> GetPublishedAsync(int page, int pageSize)
    {
        var qu = await _context.Events.Include(e=>e.TicketTypes).Where(e=>e.Status == EventStatus.Published).ToListAsync(); 
        var count = await _context.Events.CountAsync();
       var result =  qu.OrderBy(e => e.CreatedAtUtc).Skip((page - 1) * pageSize).Take(pageSize).ToList();
      
        return (result, count);
    }

    public async Task AddAsync(Event ev)
    { await _context.Events.AddAsync(ev);
        await SaveChangesAsync();
       
    }

    public async Task SaveChangesAsync()
    {

      await  _context.SaveChangesAsync();
        throw new NotImplementedException();
    }
}
