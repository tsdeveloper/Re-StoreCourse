using API.Entities;
using API.Entities.JWT;

namespace API.Extensions;

public static class JWTSettingExtensions
{
    public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JWTSettings>(configuration.GetSection(nameof(JWTSettings)));
        
        return services;
    }
}