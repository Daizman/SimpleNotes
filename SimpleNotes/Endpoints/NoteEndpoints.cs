using AutoMapper;
using SimpleNotes.Abstract;
using SimpleNotes.Dtos;
using SimpleNotes.Models.Note;

namespace SimpleNotes.Endpoints;

public static class NoteEndpoints
{
    public static void UseNoteEndpoints(this IEndpointRouteBuilder app)
    {
        var noteApi = app.MapGroup("/note").WithOpenApi();

        noteApi.MapGet("/{userId}", (
            Guid userId,
            INoteRepository noteRepo,
            IMapper mapper) =>
        {
            return Results.Ok(mapper.Map<List<ListNoteVm>>(noteRepo.GetAllForUser(userId)));
        })
            .Produces<List<ListNoteVm>>();

        noteApi.MapGet("/{userId}/{noteId}", (
            Guid userId,
            Guid noteId, 
            INoteRepository noteRepo,
            IMapper mapper) =>
        {
            var note = noteRepo.Get(userId, noteId);
            if (note is null)
                return Results.NotFound();
            return Results.Ok(mapper.Map<DetailedNoteVm>(note));
        })
            .Produces<DetailedNoteVm>()
            .Produces(StatusCodes.Status404NotFound);

        noteApi.MapPost("/{userId}", (
            Guid userId, 
            CreateNoteDto createNote, 
            INoteRepository noteRepo,
            IMapper mapper) =>
        {
            noteRepo.Add(mapper.Map<Note>((userId, createNote)));
            return Results.Created();
        })
            .Produces(StatusCodes.Status201Created);

        noteApi.MapPut("/{userId}/{noteId}", (
                Guid userId,
                Guid noteId,
                EditNoteDto editNote,
                INoteRepository noteRepo,
                IMapper mapper) =>
            {
                var result = noteRepo.Edit(mapper.Map<Note>((userId, noteId, editNote)));
                return result
                    ? Results.Ok()
                    : Results.NotFound();
            })
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

        noteApi.MapDelete("/{userId}/{noteId}", (
            Guid userId,
            Guid noteId,
            INoteRepository noteRepo) =>
        {
            return noteRepo.Remove(userId, noteId)
                ? Results.Ok()
                : Results.NotFound();
        })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }
}