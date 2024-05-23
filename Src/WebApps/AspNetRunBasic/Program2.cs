using System;
using AspNetRunBasic;
using AspNetRunBasic.Data;
using AspNetRunBasic.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add builder.Services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddDbContext<AspnetRunContext>(c =>
    c.UseSqlServer(builder.Configuration.GetConnectionString("AspnetRunConnection")));

// add repository dependecy
builder.Services.AddScoped<IProductRepository, ProductRepository>();


builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
});


builder.Services.AddRazorPages()
    .AddNewtonsoftJson();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });


app.UseHttpsRedirection();

app.SeedDatabase();
app.Run();


