using System.Reflection;
using SimpleNotes.Abstract;
using SimpleNotes.Repositories;
using SimpleNotes.Services.Common;

namespace SimpleNotes.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddSimpleNotes(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<INoteRepository, NoteRepository>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}