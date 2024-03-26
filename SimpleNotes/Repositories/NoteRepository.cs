﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SimpleNotes.Abstract;
using SimpleNotes.Dtos;
using SimpleNotes.Models.Note;

namespace SimpleNotes.Repositories;

public class NoteRepository(
    ILogger<NoteRepository> logger, 
    ISimpleNotesDbContext simpleNotesDbContext,
    IMapper mapper) : INoteRepository
{
    public async Task<DetailedNoteVm?> GetAsync(Guid userId, Guid noteId)
    {
        await ThrowIfUserNotFound(userId);

        var note = await simpleNotesDbContext
            .Notes
            .AsNoTracking()
            .FirstOrDefaultAsync(note => note.UserId == userId && note.Id == noteId);
        if (note is null)
        {
            logger.LogWarning("Attempt to get not existing note");
            return null;
        }

        return mapper.Map<DetailedNoteVm>(note);
    }

    public async Task<IReadOnlyList<ListNoteVm>> GetAllForUserAsync(Guid userId)
    {
        await ThrowIfUserNotFound(userId);

        var userNotes = await simpleNotesDbContext
            .Notes
            .AsNoTracking()
            .Where(note => note.UserId == userId)
            .ToListAsync();
        var mapped = mapper.Map<List<ListNoteVm>>(userNotes);
        return mapped.AsReadOnly();
    }

    public async Task AddAsync(Guid userId, CreateNoteDto createNoteDto)
    {
        await ThrowIfUserNotFound(userId);

        simpleNotesDbContext.Notes.Add(mapper.Map<Note>((userId, createNoteDto)));
        await simpleNotesDbContext.SaveChangesAsync();
    }

    public async Task<bool> EditAsync(Guid userId, Guid noteId, EditNoteDto editNoteDto)
    {
        await ThrowIfUserNotFound(userId);

        var note = await simpleNotesDbContext
            .Notes
            .FirstOrDefaultAsync(note => note.UserId == userId && note.Id == noteId);
        if (note is null)
        {
            logger.LogWarning("Attempt to edit not existing note");
            return false;
        }

        var newNote = mapper.Map<Note>(editNoteDto);
        note.Title = newNote.Title;
        note.Description = newNote.Description;
        note.UpdateDateTime = newNote.UpdateDateTime;
        note.IsCompleted = newNote.IsCompleted;
        note.Priority = newNote.Priority;
        
        await simpleNotesDbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveAsync(Guid userId, Guid noteId)
    {
        await ThrowIfUserNotFound(userId);
        var note = await simpleNotesDbContext
            .Notes
            .AsNoTracking()
            .FirstOrDefaultAsync(note => note.UserId == userId && note.Id == noteId);
        if (note is null)
        {
            logger.LogWarning("Attempt to edit not existing note");
            return false;
        }

        simpleNotesDbContext.Notes.Remove(note);
        await simpleNotesDbContext.SaveChangesAsync();

        return true;
    }

    private async Task ThrowIfUserNotFound(Guid userId)
    {
        var userExists = await simpleNotesDbContext.Users.AnyAsync(user => user.Id == userId);

        if (!userExists)
        {
            throw new Exception("User not found.");
        }
    }
}