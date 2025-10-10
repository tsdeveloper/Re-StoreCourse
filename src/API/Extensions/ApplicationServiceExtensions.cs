using System.Text;
using API.Data;
using API.Entities.JWT;
using API.Entities.Roles;
using API.Entities.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection service, IConfiguration config)
    {
        var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        var connString = isDevelopment
            ? config.GetConnectionString("DEV-DOCKER-POSTGRESQL")
            : config.GetConnectionString("PRD-DOCKER-POSTGRESQL");

        // service.AddDbContext<RestoreCourseDbContext>(x =>
        //     x.UseSqlServer(connString));
        
        service.AddDbContext<RestoreCourseDbContext>(x =>
            x.UseNpgsql(connString));
        service.AddCors(options =>
        {
            options.AddPolicy("CorsPolicyAllowFront",
                builder => builder.WithOrigins("http://localhost:4000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
            );
        });

        service.AddIdentityCore<UserCustom>(opt => { opt.User.RequireUniqueEmail = true; })
            .AddDefaultTokenProviders()
            .AddRoles<RoleCustom>()
            .AddEntityFrameworkStores<RestoreCourseDbContext>();

        var jwtSettings = service.BuildServiceProvider().GetService<IOptions<JWTSettings>>();

        service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.TokenKey))
                };
            });

        service.AddAuthorization();

        service.AddAutoMapper(typeof(Program));
        return service;
    }
}