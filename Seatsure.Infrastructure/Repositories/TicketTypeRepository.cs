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
        // TODO: Implement GetByIdAsync using DbContext
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<TicketType>> GetByEventIdAsync(Guid eventId)
    {
        // TODO: Implement GetByEventIdAsync using DbContext
        throw new NotImplementedException();
    }

    public async Task AddAsync(TicketType ticketType)
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
