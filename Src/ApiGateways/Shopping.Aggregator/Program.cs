using Asp.Versioning;
using Asp.Versioning.Builder;
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
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<ICatalogService, CatalogService>(c => c.BaseAddress =
    new Uri(builder.Configuration.GetValue<string>("ApiSettings:CatalogUrl") ??
            throw new ArgumentNullException(nameof(c), "api settings for order can not be null")));
builder.Services.AddHttpClient<IBasketService, BasketService>(c => c.BaseAddress =
    new Uri(builder.Configuration.GetValue<string>("ApiSettings:BasketUrl") ??
            throw new ArgumentNullException(nameof(c), "api settings for order can not be null")));
builder.Services.AddHttpClient<IOrderService, OrderService>(c => c.BaseAddress =
    new Uri(builder.Configuration.GetValue<string>("ApiSettings:OrderingUrl") ??
            throw new ArgumentNullException(nameof(c), "api settings for order can not be null")));
var app = builder.Build();
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