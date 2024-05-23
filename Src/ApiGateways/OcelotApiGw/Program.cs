using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureLogging(((context, loggingBuilder) =>
        {
            loggingBuilder.AddConfiguration(context.Configuration.GetSection("Logging"));
            loggingBuilder.AddConsole();
            loggingBuilder.AddDebug();
        }
        ));
builder.Services.AddOcelot().AddCacheManager(settings
    =>settings.WithDictionaryHandle());
var app = builder.Build();


await app.UseOcelot();
app.Run();