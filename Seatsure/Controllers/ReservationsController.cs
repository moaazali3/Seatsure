using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Seatsure.Application.DTOs.Reservations;
using Seatsure.Application.Services.Interfaces;

namespace Seatsure.Controllers;

[ApiController]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationsController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [Authorize]
    [HttpPost("api/ticket-types/{id:guid}/reservations")]
    public async Task<IActionResult> CreateHold([FromRoute] Guid id, [FromBody] CreateReservationRequest request)
    {
        var userId = GetCurrentUserId();
        var result = await _reservationService.CreateHoldAsync(id, userId, request);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [Authorize]
    [HttpPost("api/reservations/{id:guid}/confirm")]
    public async Task<IActionResult> Confirm([FromRoute] Guid id)
    {
        var userId = GetCurrentUserId();
        var result = await _reservationService.ConfirmAsync(id, userId);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("api/reservations/{id:guid}/cancel")]
    public async Task<IActionResult> Cancel([FromRoute] Guid id)
    {
        var userId = GetCurrentUserId();
        var result = await _reservationService.CancelAsync(id, userId);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("api/users/me/reservations")]
    public async Task<IActionResult> GetMyReservations()
    {
        var userId = GetCurrentUserId();
        var result = await _reservationService.GetByUserAsync(userId);
        return Ok(result);
    }

    private Guid GetCurrentUserId()
    {
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub")
            ?? throw new UnauthorizedAccessException("User claim not found.");
        return Guid.Parse(sub);
    }
}
