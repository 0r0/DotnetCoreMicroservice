using System.Data;
using Common.Logging;
using Serilog;
using Shopping.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Host.UseSerilog(SeriLogger.Configure);


builder.Services.AddTransient<LoggingDelegatingHandler>();
builder.Services.AddHttpClient<ICatalogService, CatalogService>(c =>
        c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiSettings:GatewayAddress")
                                ?? throw new NoNullAllowedException("gateway address can not be null!")))
    .AddHttpMessageHandler<LoggingDelegatingHandler>();
builder.Services.AddHttpClient<IOrderService, OrderService>(c =>
        c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiSettings:GatewayAddress")
                                ?? throw new NoNullAllowedException("gateway address can not be null!")))
    .AddHttpMessageHandler<LoggingDelegatingHandler>();
builder.Services.AddHttpClient<IBasketService, BasketService>(c =>
        c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiSettings:GatewayAddress")
                                ?? throw new NoNullAllowedException("gateway address can not be null!")))
    .AddHttpMessageHandler<LoggingDelegatingHandler>();
var app = builder.Build();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();