using Microsoft.EntityFrameworkCore;
using Seatsure.Application.Repositories;
using Seatsure.Domain;

namespace Seatsure.Infrastructure.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly AppDbContext _context;

    public ReservationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Reservation?> GetByIdAsync(Guid id)
    {
        // TODO: Implement GetByIdAsync (including TicketType) using DbContext
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Reservation>> GetByUserIdAsync(Guid userId)
    {
        // TODO: Implement GetByUserIdAsync using DbContext
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Reservation>> GetExpiredHoldsAsync()
    {
        // TODO: Implement GetExpiredHoldsAsync using DbContext
        throw new NotImplementedException();
    }

    public async Task AddAsync(Reservation reservation)
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
