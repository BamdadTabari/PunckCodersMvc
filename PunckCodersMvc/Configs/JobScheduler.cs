using DataProvider.EntityFramework.Repository;
using Hangfire;

namespace PunckCodersMvc.Configs;
public class JobScheduler
{
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly IServiceScopeFactory _scopeFactory;

    public JobScheduler(IRecurringJobManager recurringJobManager, IServiceScopeFactory scopeFactory)
    {
        _recurringJobManager = recurringJobManager;
        _scopeFactory = scopeFactory;
    }

    public void ConfigureJobs()
    {
        // Use IRecurringJobManager to schedule jobs
        _recurringJobManager.AddOrUpdate(
            "token-cleanup",
            () => ExecuteScopedTask(),
            Cron.Hourly
        );
    }

    public async Task ExecuteScopedTask()
    {
        using var scope = _scopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var task = scope.ServiceProvider.GetRequiredService<TokenCleanupTask>();
        await task.ExecuteAsync(unitOfWork);
    }
}
