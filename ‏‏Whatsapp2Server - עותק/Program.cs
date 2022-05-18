using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Whatsapp2Server.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Whatsapp2ServerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Whatsapp2ServerContext") ?? throw new InvalidOperationException("Connection string 'Whatsapp2ServerContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
