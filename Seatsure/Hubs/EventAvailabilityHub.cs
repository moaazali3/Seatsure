using Microsoft.AspNetCore.SignalR;

namespace Seatsure.Hubs;

public class EventAvailabilityHub : Hub
{
    public async Task JoinEvent(string eventId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"event-{eventId}");
    }

    public async Task LeaveEvent(string eventId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"event-{eventId}");
    }
}
