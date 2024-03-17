namespace SimpleNotes.Models.Note;

public class Note
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime CreationDateTime { get; set; }
    public DateTime UpdateDateTime { get; set; }
    public bool IsCompleted { get; set; }
    public Priority Priority { get; set; }
    
    public Guid UserId { get; set; }
}