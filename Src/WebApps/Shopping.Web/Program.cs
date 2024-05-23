using System.Data;
using Shopping.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddHttpClient<ICatalogService, CatalogService>(c=>
    c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiSettings:GatewayAddress") 
                            ?? throw new NoNullAllowedException("gateway address can not be null!")));
builder.Services.AddHttpClient<IOrderService, OrderService>(c=>
    c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiSettings:GatewayAddress") 
                            ?? throw new NoNullAllowedException("gateway address can not be null!")));
builder.Services.AddHttpClient<IBasketService, BasketService>(c=>
    c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiSettings:GatewayAddress") 
                            ?? throw new NoNullAllowedException("gateway address can not be null!")));

var app = builder.Build();

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
