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
        return await _context.TicketTypes.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<TicketType>> GetByEventIdAsync(Guid eventId)
    {
        return await _context.TicketTypes.Where(e => e.EventId == eventId).ToListAsync();
    }

    public async Task AddAsync(TicketType ticketType)
    {
        await _context.TicketTypes.AddAsync(ticketType);
    }
}
