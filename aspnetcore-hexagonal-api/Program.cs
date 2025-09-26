using aspnetcore_hexagonal_api;
using aspnetcore_hexagonal_api.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/application-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting ASP.NET Core Hexagonal API");

    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog
    builder.Host.UseSerilog();

    // Add services to the container
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.WriteIndented = true;
        });

    // Add API Explorer and Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerConfiguration();

    // Add Database
    builder.Services.AddDatabase(builder.Configuration);

    // Add Application Services
    builder.Services.AddApplicationServices();

    // Add Validators
    builder.Services.AddValidators();

    // Add CORS
    builder.Services.AddCorsConfiguration();

    // Add Health Checks
    builder.Services.AddHealthChecks()
        .AddDbContextCheck<ApplicationDbContext>();

    // Build the application
    var app = builder.Build();

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASP.NET Core Hexagonal API v1");
            c.RoutePrefix = "swagger"; // Set Swagger UI at /swagger
        });

        // Database will be created manually via EF Core migrations
        // Use: dotnet ef database update
    }
    else
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    // Use CORS
    app.UseCors(app.Environment.IsDevelopment() ? "Development" : "AllowAll");

    // Use HTTPS redirection
    app.UseHttpsRedirection();

    // Use authorization
    app.UseAuthorization();

    // Map controllers
    app.MapControllers();

    // Map health checks
    app.MapHealthChecks("/health");

    Log.Information("ASP.NET Core Hexagonal API started successfully");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
