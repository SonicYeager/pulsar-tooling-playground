﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PulsarWorker.Database.Context;

namespace PulsarWorker.Database.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection service, string connectionString)
    {
        return service
            .AddSingleton<DbContextOptions<PulsarWorkerDbContext>>(_ => new DbContextOptionsBuilder<PulsarWorkerDbContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .Options);
    }
}