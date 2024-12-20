using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;
public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection service, IConfiguration config)
    {
        var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        var connString = isDevelopment ? config.GetConnectionString("DEV-DOCKER-SQLSERVER") :
                        config.GetConnectionString("PRD-DOCKER-SQLSERVER");

        service.AddDbContext<RestoreCourseDbContext>(x =>
        x.UseSqlServer(connString));

        service.AddCors(p => {
            p.AddPolicy("CorsPolicyAllowFront", o => 
            {
                o.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4000");
            });            
        });

        service.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return service;

    }
}
