using aspnetcore_hexagonal_api.Features.FeatureExample.Adapters;
using aspnetcore_hexagonal_api.Features.FeatureExample.Application;
using aspnetcore_hexagonal_api.Features.FeatureExample.Domain.Interfaces;
using aspnetcore_hexagonal_api.Features.FeatureExample.Interfaces;
using aspnetcore_hexagonal_api.Features.FeatureExample.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace aspnetcore_hexagonal_api.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IFeatureExampleService, FeatureExampleService>();
        services.AddScoped<IFeatureExampleRepository, FeatureExampleRepository>();
        services.AddScoped<IFeatureExampleDomainService, FeatureExampleBusinessService>();

        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<Program>();

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString, b =>
                b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        return services;
    }

    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "ASP.NET Core Hexagonal API",
                Version = "v1",
                Description = "Uma API desenvolvida com arquitetura hexagonal baseada em features",
                Contact = new Microsoft.OpenApi.Models.OpenApiContact
                {
                    Name = "Development Team",
                    Email = "dev@company.com"
                }
            });

            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }
        });

        return services;
    }

    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });

            options.AddPolicy("Development",
                builder =>
                {
                    builder
                        .WithOrigins("http://localhost:3000", "https://localhost:3001")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
        });

        return services;
    }
}