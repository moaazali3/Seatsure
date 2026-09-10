using Seatsure.Application.Services.Interfaces;

namespace Seatsure.Services;

public class HoldExpiryService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<HoldExpiryService> _logger;
    private static readonly TimeSpan Interval = TimeSpan.FromSeconds(30);

    public HoldExpiryService(IServiceScopeFactory scopeFactory, ILogger<HoldExpiryService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("HoldExpiryService is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var reservationService = scope.ServiceProvider.GetRequiredService<IReservationService>();

                var expiredCount = await reservationService.ExpireHoldsAsync();
                if (expiredCount > 0)
                {
                    _logger.LogInformation("Expired {Count} overdue reservations and restored inventory.", expiredCount);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while expiring holds.");
            }

            await Task.Delay(Interval, stoppingToken);
        }

        _logger.LogInformation("HoldExpiryService is stopping.");
    }
}
