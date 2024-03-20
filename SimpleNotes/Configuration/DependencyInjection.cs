using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SimpleNotes.Abstract;
using SimpleNotes.Configuration.Mappings;
using SimpleNotes.Database;
using SimpleNotes.Repositories;
using SimpleNotes.Services.Common;
using SimpleNotes.Settings;

namespace SimpleNotes.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddSimpleNotes(this IServiceCollection services, IConfiguration configuration)
    {
        var dateTimeProvider = new DateTimeProvider();
        services.AddSingleton<IDateTimeProvider>(dateTimeProvider);
        
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile(new NoteMappingProfile(dateTimeProvider));
        });

        services.AddDbContext<SimpleNotesDbContext>(options =>
        {
            var connection = configuration.GetRequiredSection(nameof(PostgreSqlConnection)).Get<PostgreSqlConnection>();
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
        
        services.AddScoped<INoteRepository, NoteRepository>();

        return services;
    }
}