using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Seatsure.Application.Notifications;
using Seatsure.Application.Security;
using Seatsure.Application.Services;
using Seatsure.Application.Services.Interfaces;

namespace Seatsure.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the application layer: services, security, notifications, and JWT options.
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

        services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();
        services.AddSingleton<ITokenService, JwtTokenService>();

        // Replace with the SignalR-backed implementation in the API layer once the hub exists.
        services.AddSingleton<IAvailabilityNotifier, NullAvailabilityNotifier>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<ITicketTypeService, TicketTypeService>();
        services.AddScoped<IReservationService, ReservationService>();

        return services;
    }
}
