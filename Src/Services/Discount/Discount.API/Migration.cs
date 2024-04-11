using Npgsql;

namespace Discount.API;

public static class Migration
{
    public static void MigrateDatabase(this WebApplication application, int? retry = 0)
    {
        using (var scope = application.Services.CreateScope())
        {
            var retryAvailability = retry.Value;
            var services = scope.ServiceProvider;
            var configuration = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILogger<Program>>();
            try
            {
                logger.LogInformation("Migrating postgresql database");
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
            catch (NpgsqlException exception)
            {
                Console.WriteLine(exception);
                logger.LogError(exception, "an error occurred while migrating postgresql database");
                if (retryAvailability < 50)
                {
                    Thread.Sleep(2000);
                    MigrateDatabase(application, ++retryAvailability);
                }
            }
        }
    }
}