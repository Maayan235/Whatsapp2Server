using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Whatsapp2Server.Hubs;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Whatsapp2Server
{
    public static class Startup
    {

        public static WebApplication InitializeApp(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);
            var app = builder.Build();
            Configure(app);
            return app;
        }


        public static void ConfigureServices(WebApplicationBuilder builder)
        {

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(1);
            });
            builder.Services.AddSignalR();
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });




            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/Users/Login/";
                options.AccessDeniedPath = "/Users/AccessDenied/";
            });

            builder.Services.AddSignalR();
            builder.Services.AddRazorPages();
            builder.Services.AddDirectoryBrowser();
        }



        public static void Configure(WebApplication app)
        {
            app.UseRouting();
            app.UseCors();

            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
/*                endpoints.MapRazorPages();
*/                endpoints.MapHub<ChatHub>("/chat");
            });

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

          
            
            /*app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name:"default",
                    url: "{controller}/{action}",
                    defaults: new {con}
                    )
            }*/

            app.UseAuthorization();
            app.MapControllerRoute(
                 name: "default",
                 pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();



        }
        
        
    }
}


/*using Whatsapp2Server;
using Whatsapp2ServerContext = Whatsapp2Server.Whatsapp2ServerContext;*/

/*var app = Startup.InitializeApp(args);
app.Run();*/
