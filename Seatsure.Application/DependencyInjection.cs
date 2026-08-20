using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Seatsure.Application.Notifications;
using Seatsure.Application.Services;
using Seatsure.Application.Services.Interfaces;

namespace Seatsure.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration? configuration = null)
    {
        services.AddSingleton<IAvailabilityNotifier, NullAvailabilityNotifier>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<ITicketTypeService, TicketTypeService>();
        services.AddScoped<IReservationService, ReservationService>();

        return services;
    }
}
