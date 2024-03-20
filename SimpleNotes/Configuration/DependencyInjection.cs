using SimpleNotes.Abstract;
using SimpleNotes.Configuration.Mappings;
using SimpleNotes.Repositories;
using SimpleNotes.Services.Common;

namespace SimpleNotes.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddSimpleNotes(this IServiceCollection services)
    {
        var dateTimeProvider = new DateTimeProvider();
        services.AddSingleton<IDateTimeProvider>(dateTimeProvider);
        
        services.AddSingleton<INoteRepository, NoteRepository>();
        
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile(new NoteMappingProfile(dateTimeProvider));
        });

        return services;
    }
}