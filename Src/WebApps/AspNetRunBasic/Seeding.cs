using System;
using AspNetRunBasic.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AspNetRunBasic;

public static class Seeding
{
    public static void SeedDatabase(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                var aspnetRunContext = services.GetRequiredService<AspnetRunContext>();
                AspnetRunContextSeed.SeedAsync(aspnetRunContext, loggerFactory).Wait();
            }
            catch (Exception exception)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(exception, "An error occurred seeding the DB.");
            }
        }
    }
}