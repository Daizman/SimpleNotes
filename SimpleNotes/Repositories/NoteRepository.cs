using SimpleNotes.Abstract;
using SimpleNotes.Models.Note;

namespace SimpleNotes.Repositories;

public class NoteRepository(ILogger<NoteRepository> logger) : INoteRepository
{
    private readonly List<Note> _inMemoryRepo = new();
    
    public Note? Get(Guid userId, Guid id)
    {
        var note = _inMemoryRepo.FirstOrDefault(note => note.UserId == userId && note.Id == id);
        if (note is null)
        {
            logger.LogWarning("Attempt to get not existing note");
            return null;
        }

        return note;
    }

    public IReadOnlyList<Note> GetAllForUser(Guid userId)
    {
        return _inMemoryRepo.Where(note => note.UserId == userId).ToList().AsReadOnly();
    }

    public void Add(Note note)
    {
        _inMemoryRepo.Add(note);
    }

    public bool Edit(Guid userId, Guid id, Note newNote)
    {
        var note = _inMemoryRepo.FirstOrDefault(note => note.UserId == userId && note.Id == id);
        if (note is null)
        {
            logger.LogWarning("Attempt to edit not existing note");
            return false;
        }

        note.Title = newNote.Title;
        note.Description = newNote.Description;
        note.UpdateDateTime = DateTime.Now;
        note.IsCompleted = newNote.IsCompleted;
        note.Priority = newNote.Priority;

        return true;
    }

    public bool Remove(Guid userId, Guid id)
    {
        var note = _inMemoryRepo.FirstOrDefault(note => note.UserId == userId && note.Id == id);
        if (note is null)
        {
            logger.LogWarning("Attempt to edit not existing note");
            return false;
        }

        return _inMemoryRepo.Remove(note);
    }
}