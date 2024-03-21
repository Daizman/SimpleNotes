﻿using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimpleNotes.Abstract;
using SimpleNotes.Configuration.Mappings;
using SimpleNotes.Database;
using SimpleNotes.Repositories;
using SimpleNotes.Services.Auth;
using SimpleNotes.Services.Infrastructure;

namespace SimpleNotes.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddSimpleNotes(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddInfrastructure()
            .AddDatabase(configuration)
            .AddRepositories()
            .AddAuth(configuration);
        
        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        var dateTimeProvider = new DateTimeProvider();
        services.AddSingleton<IDateTimeProvider>(dateTimeProvider);
        
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile(new NoteMappingProfile(dateTimeProvider));
        });

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SimpleNotesDbContext>(options =>
        {
            var connection = configuration.GetRequiredSection(nameof(Settings.PostgreSqlConnection)).Get<Settings.PostgreSqlConnection>();
            if (connection is null)
            {
                throw new Exception("PostgreSQL connection not found in appsettings.json.");
            }
            options.UseNpgsql(connection.ConnectionString, npgsqlOptionsBuilder =>
            {
                npgsqlOptionsBuilder.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
        });
        services.AddScoped<ISimpleNotesDbContext>(provider => provider.GetRequiredService<SimpleNotesDbContext>());
        
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<INoteRepository, NoteRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        Settings.PasswordHashProvider passwordHash = new();
        configuration.Bind(nameof(Settings.PasswordHashProvider), passwordHash);
        services.AddSingleton(Options.Create(passwordHash));
        services.AddSingleton<IPasswordHashProvider, PasswordHashProvider>();
        
        Settings.JwtTokenGenerator jwtSettings = new();
        configuration.Bind(nameof(Settings.JwtTokenGenerator), jwtSettings);
        services.AddSingleton(Options.Create(jwtSettings));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        
        services.AddSingleton<IAuthService, AuthService>();

        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            });

        services.AddAuthorization();

        return services;
    }
}