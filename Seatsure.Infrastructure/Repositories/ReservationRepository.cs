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
     var res = await _context.Reservations.Include(e=>e.TicketType).Where(e=>e.Id == id).FirstOrDefaultAsync();
      return res;
    }

    public async Task<IEnumerable<Reservation>> GetByUserIdAsync(Guid userId)
    {
        var res =await _context.Reservations.Where(e => e.User.Id == userId).ToListAsync();
        return res;    
    }

    public async Task<IEnumerable<Reservation>> GetExpiredHoldsAsync()
    {
       var result = await _context.Reservations.Where(e => e.HoldExpiresAtUtc < DateTime.Now).ToListAsync();
        return result;
    }

    public async Task AddAsync(Reservation reservation)
    {
        await _context.Reservations.AddAsync(reservation);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
      
    }
}
