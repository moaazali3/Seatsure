using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Seatsure.Application.DTOs.TicketTypes;
using Seatsure.Application.Services.Interfaces;

namespace Seatsure.Controllers;

[ApiController]
public class TicketTypesController : ControllerBase
{
    private readonly ITicketTypeService _ticketTypeService;

    public TicketTypesController(ITicketTypeService ticketTypeService)
    {
        _ticketTypeService = ticketTypeService;
    }

    [HttpGet("api/events/{eventId:guid}/ticket-types")]
    public async Task<IActionResult> GetByEventId([FromRoute] Guid eventId)
    {
        var result = await _ticketTypeService.GetByEventIdAsync(eventId);
        return Ok(result);
    }

    [Authorize(Roles = "Organizer")]
    [HttpPost("api/events/{eventId:guid}/ticket-types")]
    public async Task<IActionResult> Add([FromRoute] Guid eventId, [FromBody] CreateTicketTypeRequest request)
    {
        var userId = GetCurrentUserId();
        var result = await _ticketTypeService.AddAsync(eventId, userId, request);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    private Guid GetCurrentUserId()
    {
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub")
            ?? throw new UnauthorizedAccessException("User claim not found.");
        return Guid.Parse(sub);
    }
}
