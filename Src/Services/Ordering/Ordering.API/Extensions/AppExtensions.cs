using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace Ordering.API.Extensions;

public static class AppExtensions
{
    public static void MigrateDatabase<TContext>(this WebApplication application,
        Action<TContext,IServiceProvider> seeder) where TContext:DbContext
    {
       
        using (var scope =application.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetService<TContext>();
            try
            {
                
                logger.LogInformation("Migration database with association context {DbContextName}",nameof(TContext));
                var retry = Policy.Handle<SqlException>()
                    .WaitAndRetry(
                        retryCount: 5,
                        sleepDurationProvider: (retryAttempt) => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (exception, retryCount, context) =>
                        {
                            logger.LogError($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
                        });
                    
                retry.Execute(() => InvokeSeeder(seeder, context, services));
                logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
            }
            catch (SqlException e)
            {
                logger.LogError(e,"an error occured while migrating the database using in context");
                
            }
        }
        
    }

    private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services) where TContext : DbContext
    {
        context.Database.Migrate();
        seeder(context, services);
    }
}