using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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
                options.IdleTimeout = TimeSpan.FromMinutes(10);
            });
            builder.Services.AddSignalR();
            builder.Services.AddSingleton<ChatHub>();
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

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWTParams:Audience"],
                    ValidIssuer = builder.Configuration["JWTParams:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTParams:SecretKey"]))
                };
            });



            /*            builder.Services.AddAuthentication(options =>
                        {
                            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        })
                        .AddCookie(options =>
                        {
            *//*                options.Cookie.HttpOnly = true;
                            //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                            options.Cookie.SameSite = SameSiteMode.Lax;
                            options.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;*//*
                            options.LoginPath = "/Users/Login/";
                            options.AccessDeniedPath = "/Users/AccessDenied/";
                        });*/

            builder.Services.AddSignalR();
            builder.Services.AddRazorPages();
            builder.Services.AddDirectoryBrowser();
        }



        public static void Configure(WebApplication app)
        {

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
                 name: "default",
                 pattern: "{controller=Home}/{action=Index}/{id?}");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHub<ChatHub>("/chat");
            });





            /*app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name:"default",
                    url: "{controller}/{action}",
                    defaults: new {con}
                    )
            }*/


            app.UseCors();

            app.MapRazorPages();



        }


    }
}


/*using Whatsapp2Server;
using Whatsapp2ServerContext = Whatsapp2Server.Whatsapp2ServerContext;*/

/*var app = Startup.InitializeApp(args);
app.Run();*/
