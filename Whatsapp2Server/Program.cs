
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Whatsapp2Server.Data;

using Whatsapp2Server;
//using Whatsapp2ServerContext = Whatsapp2Server.Whatsapp2ServerContext;

var app = Startup.InitializeApp(args);
app.Run();
