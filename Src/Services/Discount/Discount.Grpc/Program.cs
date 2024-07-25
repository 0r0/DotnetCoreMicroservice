using Common.Logging;
using Discount.Grpc;
using Discount.Grpc.Repositories;
using Discount.Grpc.Services;
using Serilog;

// using Discount.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<LoggingDelegatingHandler>();
builder.Host.UseSerilog(SeriLogger.Configure);
// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.MigrateDatabase();
app.Run();