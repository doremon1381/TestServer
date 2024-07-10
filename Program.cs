using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using WebApplication_forLearning.Controllers.Convention;

namespace WebApplication_forLearning
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddHttpClient();
            builder.Services.AddMvc(mvc =>
            {
                mvc.Conventions.Add(new ControllerNameAttributeConvention());
            });
            builder.Services.AddApiVersioning(v => 
            {
                v.DefaultApiVersion = new ApiVersion(1,0);
                v.AssumeDefaultVersionWhenUnspecified = true;
                v.ReportApiVersions = true;
            });

            var app = builder.Build();

            SetupPipeline(app);
            // I intentionally separate app.run with setupPipline
            // , it's not official protocol as far as I know
            app.Run();
        }

        public static void SetupPipeline(WebApplication app)
        {
            // Configure the HTTP request pipeline.

            // TODO: get runtime error
            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            // use authentication interoperates with tracking "user"'s session
            // I intent to use cookie to manage "user"'s session, so, it meen use authentication with cookie,
            // but this particular "how to do it" can be changed
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            //app.Use(async (context, next) =>
            //{
            //    // Do work that can write to the Response.
            //    await next.Invoke();
            //    // Do logging or other work that doesn't write to the Response.
            //});
        }
    }
}
