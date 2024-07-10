using DTBOAuthLoginService.Controllers.Conventions;
using DTBOAuthLoginService.Database;
using DTBOAuthLoginService.Database.Model;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace DTBOAuthLoginService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<ILAuthenticationDbContext, LAuthenticationDbContext>(optionsAction =>
            {
                optionsAction.UseSqlServer(builder.Configuration.GetConnectionString(DbUltilities.DatabaseName));
            });
            builder.Services.AddScoped<ICustomClientDbServices, CustomClientDbServices>();
            // add asp.net identity
            // will need because in identity server, need to hold login session to 
            //builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            //    .AddUserManager<IdentityUser>();
            builder.Services.AddIdentityServer()
                // for demo and test
                .AddInMemoryClients(AuthorizationResources.GetClients(builder.Configuration))
                .AddInMemoryIdentityResources(new IdentityResource[] {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                    new IdentityResources.Email(),
                    new IdentityResources.Phone(),
                });
                //.AddAspNetIdentity<CustomClient>();
            builder.Services.AddLogging(options => 
            {
                options.AddFilter("Duende", LogLevel.Debug);
            });
            builder.Services.AddApiVersioning(apiVersionOptions => 
            {
                apiVersionOptions.DefaultApiVersion = new ApiVersion(1, 0);
                apiVersionOptions.AssumeDefaultVersionWhenUnspecified = true;
                apiVersionOptions.ReportApiVersions = true;
            });
            builder.Services.AddMvc(mvcOptions => 
            {
                mvcOptions.Conventions.Add(new ControllerNameAttributeConvention());
            });

            var app = builder.Build();

            SetupPipline(app);
            // I intentionally separate app.run with setupPipline
            // , it's not official protocol as far as I know
            app.Run();
        }

        static void SetupPipline(WebApplication app)
        {
            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseIdentityServer();
            // duende IdentityServer is already have UseAuthentication,
            // so do not need to have both with app.UseAuthentication
            //app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
