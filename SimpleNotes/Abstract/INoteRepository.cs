using SimpleNotes.Models.Note;

namespace SimpleNotes.Abstract;

public interface INoteRepository
{
    Note? Get(Guid userId, Guid id);
    IReadOnlyList<Note> GetAllForUser(Guid userId);
    void Add(Note note);
    bool Edit(Guid userId, Guid id, Note newNote);
    bool Remove(Guid userId, Guid id);
}
