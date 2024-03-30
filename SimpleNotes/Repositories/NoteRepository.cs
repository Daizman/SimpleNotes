using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SimpleNotes.Abstract;
using SimpleNotes.ApiTypes;
using SimpleNotes.Errors;
using SimpleNotes.Models.Note;

namespace SimpleNotes.Repositories;

public class NoteRepository(
    ILogger<NoteRepository> logger, 
    ISimpleNotesDbContext simpleNotesDbContext,
    IMapper mapper) : INoteRepository
{
    public async Task<DetailedNoteVm> GetAsync(Guid userId, Guid noteId)
    {
        await ThrowIfUserNotFound(userId);

        var note = await GetNoteAndThrowIfNotFound(userId, noteId);

        return mapper.Map<DetailedNoteVm>(note);
    }

    public async Task<IReadOnlyList<ListNoteVm>> GetAllForUserAsync(Guid userId, OrderColumn? orderColumn = null, bool isDesc = false)
    {
        await ThrowIfUserNotFound(userId);

        var userNotesQuery = simpleNotesDbContext
            .Notes
            .AsNoTracking()
            .Where(note => note.UserId == userId);

        if (orderColumn is not null)
        {
            userNotesQuery = orderColumn switch
            {
                OrderColumn.Title => isDesc 
                    ? userNotesQuery.OrderByDescending(note => note.Title) 
                    : userNotesQuery.OrderBy(note => note.Title),
                OrderColumn.Priority => isDesc 
                    ? userNotesQuery.OrderByDescending(note => note.Priority)
                    : userNotesQuery.OrderBy(note => note.Priority),
                OrderColumn.CreationDateTime => isDesc
                    ? userNotesQuery.OrderByDescending(note => note.CreationDateTime)
                    : userNotesQuery.OrderBy(note => note.CreationDateTime),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        var userNotes = await userNotesQuery.ToListAsync();

        var mapped = mapper.Map<List<ListNoteVm>>(userNotes);
        return mapped.AsReadOnly();
    }

    public async Task AddAsync(Guid userId, CreateNoteDto createNoteDto)
    {
        await ThrowIfUserNotFound(userId);

        simpleNotesDbContext.Notes.Add(mapper.Map<Note>((userId, createNoteDto)));
        await simpleNotesDbContext.SaveChangesAsync();
    }

    public async Task EditAsync(Guid userId, Guid noteId, EditNoteDto editNoteDto)
    {
        await ThrowIfUserNotFound(userId);

        var note = await simpleNotesDbContext
            .Notes
            .FirstOrDefaultAsync(note => note.UserId == userId && note.Id == noteId);
        if (note is null)
        {
            logger.LogError("Attempt to edit not existing note");
            throw new NoteNotFoundException();
        }

        var newNote = mapper.Map<Note>(editNoteDto);
        note.Title = newNote.Title;
        note.Description = newNote.Description;
        note.UpdateDateTime = newNote.UpdateDateTime;
        note.IsCompleted = newNote.IsCompleted;
        note.Priority = newNote.Priority;
        
        await simpleNotesDbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(Guid userId, Guid noteId)
    {
        await ThrowIfUserNotFound(userId);
        var note = await GetNoteAndThrowIfNotFound(userId, noteId);

        simpleNotesDbContext.Notes.Remove(note);
        await simpleNotesDbContext.SaveChangesAsync();
    }

    private async Task ThrowIfUserNotFound(Guid userId)
    {
        var userExists = await simpleNotesDbContext.Users.AnyAsync(user => user.Id == userId);

        if (!userExists)
        {
            throw new UserNotFoundException();
        }
    }
    
    private async Task<Note> GetNoteAndThrowIfNotFound(Guid userId, Guid noteId)
    {
        var note = await simpleNotesDbContext
            .Notes
            .AsNoTracking()
            .FirstOrDefaultAsync(note => note.UserId == userId && note.Id == noteId);

        if (note is null)
        {
            throw new NoteNotFoundException();
        }

        return note;
    }
}