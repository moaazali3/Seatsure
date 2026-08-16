using Microsoft.EntityFrameworkCore;
using Seatsure.Application.Repositories;
using Seatsure.Domain;

namespace Seatsure.Infrastructure.Repositories;

public class TicketTypeRepository : ITicketTypeRepository
{
    private readonly AppDbContext _context;

    public TicketTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TicketType?> GetByIdAsync(Guid id)
    {

        var result = await _context.TicketTypes.Where(e => e.Id == id).FirstOrDefaultAsync();

        return result;
    }

    public async Task<IEnumerable<TicketType>> GetByEventIdAsync(Guid eventId)
    {
        var result = await _context.TicketTypes.Where(e => e.EventId == eventId).ToListAsync();
        return result;
    }

    public async Task AddAsync(TicketType ticketType)
    {
        await _context.TicketTypes.AddAsync(ticketType);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
