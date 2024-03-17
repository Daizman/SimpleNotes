using AutoMapper;
using SimpleNotes.Abstract;
using SimpleNotes.Dtos;

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
            return Results.Ok(mapper.Map<NotesVm>(noteRepo.GetAllForUser(userId)));
        })
            .Produces<NotesVm>();

        noteApi.MapGet("/{userId}/{noteId}", (
            Guid userId,
            Guid noteId, 
            INoteRepository noteRepo,
            IMapper mapper) =>
        {
            var note = noteRepo.Get(userId, noteId);
            if (note is null)
                return Results.NotFound();
            return Results.Ok<DetailedNoteVm>(mapper.Map<DetailedNoteVm>(note));
        })
            .Produces<DetailedNoteVm>()
            .Produces(StatusCodes.Status404NotFound);

        // toDo: post, put, delete
        noteApi.MapPost("/{userId}", (Guid userId, CreateNoteDto createNote) =>
        {
            
        });
    }
}