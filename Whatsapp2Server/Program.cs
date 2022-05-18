using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Whatsapp2Server.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Whatsapp2ServerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Whatsapp2ServerContext") ?? throw new InvalidOperationException("Connection string 'Whatsapp2ServerContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1);
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options => {
    options.LoginPath = "/Users/Login/";
    options.AccessDeniedPath = "/Users/AccessDenied/";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseSession();
app.UseAuthentication();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "~/index.html");
    //"{controller=Users}/{action=Login}/{id?}");

app.Run();
