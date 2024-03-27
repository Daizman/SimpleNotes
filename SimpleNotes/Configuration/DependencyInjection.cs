using System.Reflection;
using System.Security.Claims;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimpleNotes.Abstract;
using SimpleNotes.Configuration.Mappings;
using SimpleNotes.Configuration.Policies;
using SimpleNotes.Database;
using SimpleNotes.Errors;
using SimpleNotes.Repositories;
using SimpleNotes.Services.Auth;
using SimpleNotes.Services.Infrastructure;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SimpleNotes.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddSimpleNotes(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddErrorHandle()
            .AddOptions()
            .AddInfrastructure()
            .AddDatabase(configuration)
            .AddRepositories()
            .AddAuth(configuration);
        
        return services;
    }

    private static IServiceCollection AddErrorHandle(this IServiceCollection services)
    {
        services.AddProblemDetails();

        return services;
    }

    private static IServiceCollection AddOptions(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        var dateTimeProvider = new DateTimeProvider();
        services.AddSingleton<IDateTimeProvider>(dateTimeProvider);
        
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile(new NoteMappingProfile(dateTimeProvider));
            cfg.AddProfile(new UserMappingProfile(dateTimeProvider));
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SimpleNotesDbContext>(options =>
        {
            var connection = configuration.GetRequiredSection(nameof(Settings.PostgreSqlConnection)).Get<Settings.PostgreSqlConnection>();
            if (connection is null)
            {
                throw new PostgreSqlConnectionStringException();
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
        services.AddTransient<IPasswordHashProvider, PasswordHashProvider>();
        
        Settings.JwtTokenGenerator jwtSettings = new();
        configuration.Bind(nameof(Settings.JwtTokenGenerator), jwtSettings);
        services.AddSingleton(Options.Create(jwtSettings));
        services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();
        
        services.AddScoped<IAuthService, AuthService>();

        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var cache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
                        var userId = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        if (userId is null 
                            || !Guid.TryParse(userId, out var guid) 
                            || !cache.TryGetValue(guid, out var token)
                            || token.ToString() != context.SecurityToken.UnsafeToString())
                        {
                            context.Fail("Unauthorized");
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        
        services.AddSingleton<IAuthorizationHandler, NotesOwnerRequirementHandler>();

        services.AddAuthorization(options =>
        {
            var defaultAuthorizationPolicyBuilder =
                new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
            defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
            options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            
            options.AddPolicy("NotesOwner", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.AddRequirements(new NotesOwnerRequirement());
            });
        });

        return services;
    }
}