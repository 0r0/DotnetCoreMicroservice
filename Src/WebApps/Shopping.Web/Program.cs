using System.Data;
using System.Reflection;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Elastic.Transport;
using Serilog;
using Shopping.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(new[] {new Uri(context.Configuration["ElasticConfiguration:Uri"])}, opt =>
            {
                opt.DataStream = new DataStreamName("logs", "appLogs", Assembly.GetExecutingAssembly()
                                                                           .GetName().Name.ToLower()
                                                                           .Replace(".", "-") + "-" +
                                                                       context.HostingEnvironment.EnvironmentName
                                                                           .ToLower().Replace(".", "-") + "-logs-" +
                                                                       "" + DateTime.UtcNow.ToString("yyyy-MM"));
                opt.BootstrapMethod = BootstrapMethod.Failure;
            },
            transport =>
            {
                transport.Authentication(new BasicAuthentication(context.Configuration["ElasticConfiguration:UserName"],
                    context.Configuration["ElasticConfiguration:Password"]));
            }).Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
        .ReadFrom.Configuration(context.Configuration);
});


builder.Services.AddHttpClient<ICatalogService, CatalogService>(c =>
    c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiSettings:GatewayAddress")
                            ?? throw new NoNullAllowedException("gateway address can not be null!")));
builder.Services.AddHttpClient<IOrderService, OrderService>(c =>
    c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiSettings:GatewayAddress")
                            ?? throw new NoNullAllowedException("gateway address can not be null!")));
builder.Services.AddHttpClient<IBasketService, BasketService>(c =>
    c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiSettings:GatewayAddress")
                            ?? throw new NoNullAllowedException("gateway address can not be null!")));
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