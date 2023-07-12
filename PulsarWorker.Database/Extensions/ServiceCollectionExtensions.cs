using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PulsarWorker.Database.Context;

namespace PulsarWorker.Database.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection service, string connectionString)
    {
        return service
            .AddSingleton<DbContextOptions<PulsarWorkerDbContext>>(_ => new DbContextOptionsBuilder<PulsarWorkerDbContext>()
                .UseLoggerFactory(LoggerFactory.Create(static builder =>
                {
                    builder
                        .AddDebug()
                        .AddConsole();
                }))
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .Options)
            .AddDbContextPool<PulsarWorkerDbContext>(o => o
                .UseLoggerFactory(LoggerFactory.Create(static builder =>
                {
                    builder
                        .AddDebug()
                        .AddConsole();
                }))
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
    }
}