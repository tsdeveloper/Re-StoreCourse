using Serilog;

namespace API.Extensions;

public static class ApplicationLogExtensions
{
  public static void AddApplicationLog(this IHostBuilder b)
  {
    b.UseSerilog((context, config) => {
      config.ReadFrom.Configuration(context.Configuration);
    });
  }
}
