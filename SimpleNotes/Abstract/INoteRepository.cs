using SimpleNotes.Dtos;
using SimpleNotes.Models.Note;

namespace SimpleNotes.Abstract;

public interface INoteRepository
{
    Task<DetailedNoteVm?> GetAsync(Guid userId, Guid noteId);
    Task<IReadOnlyList<ListNoteVm>> GetAllForUserAsync(Guid userId);
    Task AddAsync(Guid userId, CreateNoteDto createNoteDto);
    Task<bool> EditAsync(Guid userId, Guid noteId, EditNoteDto editNoteDto);
    Task<bool> RemoveAsync(Guid userId, Guid noteId);
}
