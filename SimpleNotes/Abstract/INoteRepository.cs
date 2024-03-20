using SimpleNotes.Models.Note;

namespace SimpleNotes.Abstract;

public interface INoteRepository
{
    Task<Note?> GetAsync(Guid userId, Guid id);
    Task<IReadOnlyList<Note>> GetAllForUserAsync(Guid userId);
    Task AddAsync(Note note);
    Task<bool> EditAsync(Note newNote);
    Task<bool> RemoveAsync(Guid userId, Guid id);
}
