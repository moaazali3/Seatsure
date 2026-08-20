using Seatsure.Application.DTOs.Reservations;
using Seatsure.Application.Exceptions;
using Seatsure.Application.Notifications;
using Seatsure.Application.Repositories;
using Seatsure.Application.Services.Interfaces;
using Seatsure.Domain;

namespace Seatsure.Application.Services;

internal sealed class ReservationService : IReservationService
{
    private const int HoldMinutes = 10;

    private readonly IReservationRepository _reservations;
    private readonly ITicketTypeRepository _ticketTypes;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAvailabilityNotifier _notifier;

    public ReservationService(
        IReservationRepository reservations,
        ITicketTypeRepository ticketTypes,
        IUnitOfWork unitOfWork,
        IAvailabilityNotifier notifier)
    {
        _reservations = reservations;
        _ticketTypes = ticketTypes;
        _unitOfWork = unitOfWork;
        _notifier = notifier;
    }

    public async Task<ReservationDto> CreateHoldAsync(Guid ticketTypeId, Guid userId, CreateReservationRequest request)
    {
        if (request.Quantity < 1)
            throw new ValidationException("Quantity must be at least 1.");

        var ticketType = await _ticketTypes.GetByIdAsync(ticketTypeId)
            ?? throw new NotFoundException($"Ticket type {ticketTypeId} was not found.");

        if (request.Quantity > ticketType.AvailableQuantity)
            throw new ConflictException("Insufficient inventory for the requested quantity.");

        ticketType.AvailableQuantity -= request.Quantity;

        var reservation = new Reservation
        {
            TicketTypeId = ticketTypeId,
            UserId = userId,
            Quantity = request.Quantity,
            Status = ReservationStatus.Pending,
            HoldExpiresAtUtc = DateTime.UtcNow.AddMinutes(HoldMinutes),
            CreatedAtUtc = DateTime.UtcNow
        };
        await _reservations.AddAsync(reservation);

        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (ConflictException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new ConflictException("Someone booked first. Please retry.");
        }

        await _notifier.AvailabilityChangedAsync(ticketTypeId, ticketType.AvailableQuantity);
        return reservation.ToDto();
    }

    public async Task<ReservationDto> ConfirmAsync(Guid reservationId, Guid userId)
    {
        var reservation = await _reservations.GetByIdAsync(reservationId)
            ?? throw new NotFoundException($"Reservation {reservationId} was not found.");

        if (reservation.UserId != userId)
            throw new ForbiddenException("You can only confirm your own reservations.");

        if (reservation.Status != ReservationStatus.Pending)
            throw new ConflictException($"Reservation cannot be confirmed from status {reservation.Status}.");

        if (reservation.HoldExpiresAtUtc < DateTime.UtcNow)
            throw new ConflictException("The hold has expired.");

        reservation.Status = ReservationStatus.Confirmed;
        reservation.ConfirmedAtUtc = DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();

        await _notifier.AvailabilityChangedAsync(reservation.TicketTypeId, reservation.TicketType.AvailableQuantity);
        return reservation.ToDto();
    }

    public async Task<ReservationDto> CancelAsync(Guid reservationId, Guid userId)
    {
        var reservation = await _reservations.GetByIdAsync(reservationId)
            ?? throw new NotFoundException($"Reservation {reservationId} was not found.");

        if (reservation.UserId != userId)
            throw new ForbiddenException("You can only cancel your own reservations.");

        if (reservation.Status is not (ReservationStatus.Pending or ReservationStatus.Confirmed))
            throw new ConflictException($"Reservation cannot be cancelled from status {reservation.Status}.");

        reservation.Status = ReservationStatus.Cancelled;
        reservation.TicketType.AvailableQuantity += reservation.Quantity;
        await _unitOfWork.SaveChangesAsync();

        await _notifier.AvailabilityChangedAsync(reservation.TicketTypeId, reservation.TicketType.AvailableQuantity);
        return reservation.ToDto();
    }

    public async Task<IEnumerable<ReservationDto>> GetByUserAsync(Guid userId)
    {
        var reservations = await _reservations.GetByUserIdAsync(userId);
        return reservations.Select(r => r.ToDto());
    }

    public async Task<int> ExpireHoldsAsync()
    {
        var expired = (await _reservations.GetExpiredHoldsAsync()).ToList();
        if (expired.Count == 0)
            return 0;

        foreach (var reservation in expired)
        {
            reservation.Status = ReservationStatus.Expired;
            reservation.TicketType.AvailableQuantity += reservation.Quantity;
        }

        await _unitOfWork.SaveChangesAsync();

        foreach (var reservation in expired)
            await _notifier.AvailabilityChangedAsync(reservation.TicketTypeId, reservation.TicketType.AvailableQuantity);

        return expired.Count;
    }
}
