using AutoMapper;
using SimpleNotes.Abstract;
using SimpleNotes.Dtos;
using SimpleNotes.Filters;

namespace SimpleNotes.Endpoints;

public static class NoteEndpoints
{
    public static void MapNoteEndpoints(this IEndpointRouteBuilder app)
    {
        var noteApi = app.MapGroup("/note")
            .WithOpenApi()
            .RequireAuthorization()
            .AddEndpointFilter<AuthorizationFilter>();

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
            .AddEndpointFilter<CreateNoteDtoValidationFilter>()
            .Produces(StatusCodes.Status201Created);

        noteApi.MapPut("/{userId}/{noteId}", async (
                Guid userId,
                Guid noteId,
                EditNoteDto editNote,
                INoteRepository noteRepo) =>
            {
                await noteRepo.EditAsync(userId, noteId, editNote);
                return Results.NoContent();
            })
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound);

        noteApi.MapDelete("/{userId}/{noteId}", async (
            Guid userId,
            Guid noteId,
            INoteRepository noteRepo) =>
        {
            await noteRepo.RemoveAsync(userId, noteId);
            return Results.NoContent();
        })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }
}