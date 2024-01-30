using static Catalog.API.Endpoints;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.CatalogRoutes();
app.Run();