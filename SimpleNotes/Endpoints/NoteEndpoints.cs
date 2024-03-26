using AutoMapper;
using SimpleNotes.Abstract;
using SimpleNotes.Dtos;

namespace SimpleNotes.Endpoints;

public static class NoteEndpoints
{
    public static void UseNoteEndpoints(this IEndpointRouteBuilder app)
    {
        var noteApi = app.MapGroup("/note")
            .WithOpenApi()
            .RequireAuthorization();

        noteApi.MapGet("/{userId}", async (
            Guid userId,
            INoteRepository noteRepo,
            IMapper mapper) =>
        {
            return Results.Ok(mapper.Map<List<ListNoteVm>>(await noteRepo.GetAllForUserAsync(userId)));
        })
            .Produces<List<ListNoteVm>>();

        noteApi.MapGet("/{userId}/{noteId}", async (
            Guid userId,
            Guid noteId, 
            INoteRepository noteRepo) =>
        {
            var note = await noteRepo.GetAsync(userId, noteId);
            if (note is null)
                return Results.NotFound();
            return Results.Ok(note);
        })
            .Produces<DetailedNoteVm>()
            .Produces(StatusCodes.Status404NotFound);

        noteApi.MapPost("/{userId}", async (
            Guid userId, 
            CreateNoteDto createNote, 
            INoteRepository noteRepo) =>
        {
            await noteRepo.AddAsync(userId, createNote);
            return Results.Created();
        })
            .Produces(StatusCodes.Status201Created);

        noteApi.MapPut("/{userId}/{noteId}", async (
                Guid userId,
                Guid noteId,
                EditNoteDto editNote,
                INoteRepository noteRepo) =>
            {
                var result = await noteRepo.EditAsync(userId, noteId, editNote);
                return result
                    ? Results.Ok()
                    : Results.NotFound();
            })
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

        noteApi.MapDelete("/{userId}/{noteId}", async (
            Guid userId,
            Guid noteId,
            INoteRepository noteRepo) =>
        {
            return await noteRepo.RemoveAsync(userId, noteId)
                ? Results.Ok()
                : Results.NotFound();
        })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }
}