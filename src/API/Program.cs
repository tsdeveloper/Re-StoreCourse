using System.Text.Json;
using System.Text.Json.Serialization;
using API.Data;
using API.Entities;
using API.Entities.Roles;
using API.Entities.Users;
using API.Extensions;
using API.Middleware;
using API.Seed;
using API.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
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
    
    builder.Services.AddConfig(builder.Configuration);
    
    var serviceProvider = builder.Services.BuildServiceProvider();
    var conf = serviceProvider.GetRequiredService<IConfiguration>();

    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddControllers()
    .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        })
    .AddNewtonsoftJson(options => {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        // options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.All;
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    });

    builder.Services.AddSerilog();
    builder.Services.AddScoped<TokenService>();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        var jwtSecurityScheme = new OpenApiSecurityScheme
        {
            BearerFormat = "JWT",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Description = "Put Bearer + your token in the box below",
            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };
        
        c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                jwtSecurityScheme, Array.Empty<string>()
            }
        });
    });
    builder.Services.AddScoped<PaymentService>();
    
    builder.Services.AddApplicationServices(conf);

    var app = builder.Build();
    
    app.UseMiddleware<ExceptionMiddleware>();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
        });
    }

    app.UseCors("CorsPolicyAllowFront");
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseStaticFiles();
    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.MapControllers();

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<RestoreCourseDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserCustom>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleCustom>>();
    await context.Database.MigrateAsync();

    if (true)
        await RestoreCourseContextSeed.SeedAsync(context, userManager, roleManager);

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
