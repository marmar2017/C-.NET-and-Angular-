using coffeeOrder.Data;
using coffeeOrder.Models;
using Microsoft.EntityFrameworkCore;

namespace coffeeOrder.Services
{
    public class CoffeeOrderHostedService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<CoffeeOrderHostedService> _logger;
        private Timer? _timer;

        public CoffeeOrderHostedService(IServiceProvider services, ILogger<CoffeeOrderHostedService> logger)
        {
            _services = services;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(" CoffeeOrderHostedService started.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    using var scope = _services.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<CoffeeOrderDbContext>();

                    _logger.LogInformation("Checking for pending orders...");

                    var pendingOrders = await db.Orders
                        .Where(o => o.Status == OrderStatus.Pending)
                        .ToListAsync();

                    _logger.LogInformation($"📦 Found {pendingOrders.Count} pending orders.");

                    foreach (var order in pendingOrders)
                    {
                        try
                        {
                            order.Status = OrderStatus.InProgress;
                            order.UpdatedAt = DateTime.UtcNow;
                            await db.SaveChangesAsync();

                            _logger.LogInformation($"Processing order {order.Id}");

                            // Simulate coffee preparation
                            await Task.Delay(Random.Shared.Next(1000, 6000));

                            // Simulate failure randomly 
                            bool fail = Random.Shared.NextDouble() < 0.2;
                            //bool fail = true;
                            if (fail)
                                throw new Exception("🔥 Simulated preparation failure.");

                            order.Status = OrderStatus.Ready;
                            order.CompletedAt = DateTime.UtcNow;
                            order.UpdatedAt = DateTime.UtcNow;

                            _logger.LogInformation($"✅ Order {order.Id} is ready.");
                        }
                        catch (Exception ex)
                        {
                            order.RetryCount++;
                            order.UpdatedAt = DateTime.UtcNow;

                            if (order.RetryCount >= 3)
                            {
                                order.Status = OrderStatus.Failed;
                                order.CompletedAt = DateTime.UtcNow;
                                _logger.LogError($"❌ Order {order.Id} failed after 3 retries. Error: {ex.Message}");
                            }
                            else
                            {
                                order.Status = OrderStatus.Pending;
                                _logger.LogWarning($"⚠️ Retry {order.RetryCount} for order {order.Id}. Error: {ex.Message}");
                            }
                        }

                        await db.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "🔥 Unhandled exception in CoffeeOrderHostedService DoWork.");
                }
            });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("🛑 CoffeeOrderHostedService stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
