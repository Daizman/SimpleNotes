using SimpleNotes.Abstract;

namespace SimpleNotes.Services.Common;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
}