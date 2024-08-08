using Npgsql;
using Polly;

namespace Discount.API;

public static class Migration
{
    public static void MigrateDatabase(this WebApplication application)
    {
        using (var scope = application.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var configuration = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILogger<Program>>();
            try
            {
                logger.LogInformation("Migrating postgresql database");
                var retry = Policy.Handle<NpgsqlException>()
                    .WaitAndRetry(
                        retryCount: 5,
                        sleepDurationProvider: (retryAttempt) => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (exception, retryCount, context) =>
                        {
                            logger.LogError(
                                $"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
                        });

                retry.Execute(() => ExecuteMigrations(configuration));
            }
            catch (NpgsqlException exception)
            {
                Console.WriteLine(exception);
                logger.LogError(exception, "an error occurred while migrating postgresql database");
            }
        }
    }

    private static void ExecuteMigrations(IConfiguration configuration)
    {
        using var connection =
            new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        connection.Open();
        using var command = new NpgsqlCommand()
        {
            Connection = connection
        };

        command.CommandText = "DROP TABLE  IF EXISTS COUPON";
        command.ExecuteNonQuery();
        command.CommandText =
            "CREATE TABLE COUPON(Id SERIAL PRIMARY KEY ,ProductName VARCHAR(24) NOT NULL ,Description TEXT," +
            "Amount INT)";
        command.ExecuteNonQuery();

        command.CommandText =
            "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);";
        command.ExecuteNonQuery();

        command.CommandText =
            "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";
        command.ExecuteNonQuery();
    }
}