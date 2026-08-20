using Seatsure.Domain;

namespace Seatsure.Application.Repositories;

public interface IReservationRepository
{
    Task<Reservation?> GetByIdAsync(Guid id);
    Task<IEnumerable<Reservation>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<Reservation>> GetExpiredHoldsAsync();
    Task AddAsync(Reservation reservation);
}
