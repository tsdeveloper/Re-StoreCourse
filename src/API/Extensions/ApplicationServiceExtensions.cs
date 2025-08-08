using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;
public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection service, IConfiguration config)
    {
        var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        var connString = isDevelopment
            ? config.GetConnectionString("DEV-DOCKER-SQLSERVER")
            : config.GetConnectionString("PRD-DOCKER-SQLSERVER");

        service.AddDbContext<RestoreCourseDbContext>(x =>
            x.UseSqlServer(connString));

        service.AddCors(options =>
        {
            options.AddPolicy("CorsPolicyAllowFront",
                builder => builder.WithOrigins("http://localhost:4000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                );
        });

        service.AddIdentityCore<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<RestoreCourseDbContext>();

        service.AddAuthentication();
        service.AddAuthorization();

    service.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        return service;
    }
}
