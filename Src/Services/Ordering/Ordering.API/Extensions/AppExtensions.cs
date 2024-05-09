using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ordering.API.Extensions;

public static class AppExtensions
{
    public static void MigrateDatabase<TContext>(this WebApplication application,
        Action<TContext,IServiceProvider> seeder,int? retry = 0) where TContext:DbContext
    {
        int retryForAvailability = retry.Value;
        using (var scope =application.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetService<TContext>();
            try
            {
                logger.LogInformation("Migration database with association context {DbContextName}",nameof(TContext));
                InvokeSeeder(seeder,context,services);
            }
            catch (SqlException e)
            {
                logger.LogError(e,"an error occured while migrating the database using in context");
                if (retryForAvailability < 50)
                {
                    retryForAvailability++;
                    Thread.Sleep(2000);
                    MigrateDatabase(application,seeder,retryForAvailability);
                }
            }
        }
        
    }

    private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services) where TContext : DbContext
    {
        context.Database.Migrate();
        seeder(context, services);
    }
}