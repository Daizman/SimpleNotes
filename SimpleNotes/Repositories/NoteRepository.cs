using Microsoft.EntityFrameworkCore;
using SimpleNotes.Abstract;
using SimpleNotes.Models.Note;

namespace SimpleNotes.Repositories;

public class NoteRepository(ILogger<NoteRepository> logger, ISimpleNotesDbContext simpleNotesDbContext) : INoteRepository
{
    public async Task<Note?> GetAsync(Guid userId, Guid id)
    {
        var note = await simpleNotesDbContext.Notes.FirstOrDefaultAsync(note => note.UserId == userId && note.Id == id);
        if (note is null)
        {
            logger.LogWarning("Attempt to get not existing note");
            return null;
        }

        return note;
    }

    public async Task<IReadOnlyList<Note>> GetAllForUserAsync(Guid userId)
    {
        return (await simpleNotesDbContext.Notes.Where(note => note.UserId == userId).ToListAsync()).AsReadOnly();
    }

    public async Task AddAsync(Note note)
    {
        simpleNotesDbContext.Notes.Add(note);
        await simpleNotesDbContext.SaveChangesAsync();
    }

    public async Task<bool> EditAsync(Note newNote)
    {
        var note = await simpleNotesDbContext.Notes.FirstOrDefaultAsync(note =>
            note.UserId == newNote.UserId && note.Id == newNote.Id);
        if (note is null)
        {
            logger.LogWarning("Attempt to edit not existing note");
            return false;
        }

        note.Title = newNote.Title;
        note.Description = newNote.Description;
        note.UpdateDateTime = newNote.UpdateDateTime;
        note.IsCompleted = newNote.IsCompleted;
        note.Priority = newNote.Priority;
        
        await simpleNotesDbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveAsync(Guid userId, Guid id)
    {
        var note = await simpleNotesDbContext.Notes.FirstOrDefaultAsync(note => note.UserId == userId && note.Id == id);
        if (note is null)
        {
            logger.LogWarning("Attempt to edit not existing note");
            return false;
        }

        simpleNotesDbContext.Notes.Remove(note);
        await simpleNotesDbContext.SaveChangesAsync();

        return true;
    }
}