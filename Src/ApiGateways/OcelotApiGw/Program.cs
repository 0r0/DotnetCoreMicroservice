using Common.Logging;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);
builder.Configuration.AddJsonFile("ocelot.json", true, true);
builder.Services.AddTransient<LoggingDelegatingHandler>();
builder.Host.UseSerilog(SeriLogger.Configure);
builder.Services.AddOcelot().AddCacheManager(settings
    => settings.WithDictionaryHandle());
var app = builder.Build();


app.UseRouting();

app.UseEndpoints(_ => { });
await app.UseOcelot();
app.Run();