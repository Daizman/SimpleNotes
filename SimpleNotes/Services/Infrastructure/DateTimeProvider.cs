using SimpleNotes.Abstract;

namespace SimpleNotes.Services.Infrastructure;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}