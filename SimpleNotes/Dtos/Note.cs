namespace SimpleNotes.Dtos;

public record CreateNoteDto(string Title, string? Description, bool IsCompleted, string Priority);

public record EditNoteDto(Guid Id, string Title, string? Description, bool IsCompleted, string Priority);

public record ListNoteVm(Guid Id, string Title, bool IsCompleted, string Priority);

public record NotesVm(List<ListNoteVm> Notes);

public record DetailedNoteVm(Guid Id, string Title, string? Description, bool IsCompleted, string Priority);
