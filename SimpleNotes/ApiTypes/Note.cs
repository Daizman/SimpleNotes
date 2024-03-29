namespace SimpleNotes.ApiTypes;

public record CreateNoteDto(string Title, string? Description, bool IsCompleted, string Priority);

public record EditNoteDto(string Title, string? Description, bool IsCompleted, string Priority);

public record ListNoteVm(Guid Id, string Title, bool IsCompleted, string Priority);

public record DetailedNoteVm(Guid Id, string Title, string? Description, bool IsCompleted, string Priority);

public enum OrderColumn
{
    Title,
    Priority,
    CreationDateTime
}