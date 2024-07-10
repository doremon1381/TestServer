using DatabaseApiServices.Services;
using Microsoft.EntityFrameworkCore;

namespace DatabaseApiServices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<IHSRWarehouseDbContext, HSRWarehouseDbContext>(builderOptions => {
                builderOptions.UseSqlServer(builder.Configuration.GetConnectionString(DbUltilities.DbName));
            });

            var app = builder.Build();
            
            SetupPipline(app);

            app.Run();
        }

        private static void SetupPipline(WebApplication app)
        {
            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
