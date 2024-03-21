namespace SimpleNotes.Abstract;

public interface IDateTimeProvider
{ 
    DateTime UtcNow { get; }
}