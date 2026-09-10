using Microsoft.AspNetCore.SignalR;
using Seatsure.Application.Notifications;
using Seatsure.Hubs;

namespace Seatsure.Services;

public class SignalRAvailabilityNotifier : IAvailabilityNotifier
{
    private readonly IHubContext<EventAvailabilityHub> _hubContext;

    public SignalRAvailabilityNotifier(IHubContext<EventAvailabilityHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task AvailabilityChangedAsync(Guid ticketTypeId, int availableQuantity)
    {
        // Broadcasts to all connected clients or specific event listeners
        await _hubContext.Clients.All.SendAsync("AvailabilityChanged", ticketTypeId, availableQuantity);
    }
}
