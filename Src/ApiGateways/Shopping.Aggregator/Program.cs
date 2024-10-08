using Asp.Versioning;
using Asp.Versioning.Builder;
using Common.Logging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Polly;
using Serilog;
using Shopping.Aggregator;
using Shopping.Aggregator.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"));
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddTransient<LoggingDelegatingHandler>();
builder.Host.UseSerilog(SeriLogger.Configure);

builder.Services.AddSwaggerGen();
builder.Services.AddTransient<LoggingDelegatingHandler>();

builder.Services.AddHttpClient<ICatalogService, CatalogService>(c => c.BaseAddress =
        new Uri(builder.Configuration.GetValue<string>("ApiSettings:CatalogUrl") ??
                throw new ArgumentNullException(nameof(c), "api settings for order can not be null")))
    .AddHttpMessageHandler<LoggingDelegatingHandler>();

builder.Services.AddHttpClient<IBasketService, BasketService>(c => c.BaseAddress =
        new Uri(builder.Configuration.GetValue<string>("ApiSettings:BasketUrl") ??
                throw new ArgumentNullException(nameof(c), "api settings for order can not be null")))
    .AddHttpMessageHandler<LoggingDelegatingHandler>()
    .AddTransientHttpErrorPolicy(policy =>
        policy.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2)))
    .AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

builder.Services.AddHttpClient<IOrderService, OrderService>(c => c.BaseAddress =
        new Uri(builder.Configuration.GetValue<string>("ApiSettings:OrderingUrl") ??
                throw new ArgumentNullException(nameof(c), "api settings for order can not be null")))
    .AddHttpMessageHandler<LoggingDelegatingHandler>();

builder.Services.AddHealthChecks().AddUrlGroup(
    new Uri(builder.Configuration.GetValue<string>("ApiSettings:CatalogUrl") + "/swagger/Index.html")
    , "Catalog.API", HealthStatus.Degraded)
    .AddUrlGroup(new Uri(builder.Configuration.GetValue<string>("ApiSettings:OrderingUrl") + "/swagger/Index.html")
        , "Ordering.API", HealthStatus.Degraded)
    .AddUrlGroup(new Uri(builder.Configuration.GetValue<string>("ApiSettings:BasketUrl") + "/swagger/Index.html")
        , "Basket.API", HealthStatus.Degraded);


var app = builder.Build();
app.MapHealthChecks("/hc",new HealthCheckOptions()
{
    Predicate = _=>true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
ApiVersionSet apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .ReportApiVersions()
    .Build();
app.ShoppingAggregatorRoutes(apiVersionSet);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();