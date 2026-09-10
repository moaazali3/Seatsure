using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Seatsure.Application.DTOs.Events;
using Seatsure.Application.Services.Interfaces;

namespace Seatsure.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPublished([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _eventService.GetPublishedAsync(page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await _eventService.GetByIdAsync(id);
        return Ok(result);
    }

    [Authorize(Roles = "Organizer")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEventRequest request)
    {
        var userId = GetCurrentUserId();
        var result = await _eventService.CreateAsync(userId, request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [Authorize(Roles = "Organizer")]
    [HttpPost("{id:guid}/publish")]
    public async Task<IActionResult> Publish([FromRoute] Guid id)
    {
        var userId = GetCurrentUserId();
        var result = await _eventService.PublishAsync(id, userId);
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
