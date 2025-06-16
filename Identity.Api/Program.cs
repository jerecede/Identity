using Identity.Api.Services;
using Identity.Api.Services.Interfaces;
using Identity.Api.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Identity.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(builder.Configuration)
                            .CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v3", new OpenApiInfo
                {
                    Title = "Identity API",
                    Version = "v3",
                    Description = "only contains User."
                });
            });

            // Add services to the container.

            builder.Services.AddDbContext<IdentityContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddControllers();
            builder.Services.AddScoped<IUsersService, JereUsersService>();
            builder.Services.AddScoped<IRequestService, JereRequestService>();
            builder.Services.AddScoped<IRoleService, JereRoleService>();

            var app = builder.Build();

            app.UseSwagger(c =>
            {
                c.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v3/swagger.json", "Identity API V3");
            });

            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
