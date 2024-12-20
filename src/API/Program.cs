using System.Text.Json.Serialization;
using API.Data;
using API.Extensions;
using API.Seed;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);
    Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

    Log.Information("Starting up");
    var serviceProvider = builder.Services.BuildServiceProvider();
    var conf = serviceProvider.GetRequiredService<IConfiguration>();

    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddControllers()
    .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        })
    .AddNewtonsoftJson(options => {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });
    builder.Services.AddSerilog();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddApplicationServices(conf);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseStaticFiles();
    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.MapControllers();
    app.UseCors("CorsPolicyAllowFront");

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<RestoreCourseDbContext>();
    await context.Database.MigrateAsync();

    if (true)
        await RestoreCourseContextSeed.SeedAsync(context);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "App down");
}
finally
{
    await Log.CloseAndFlushAsync();
}
