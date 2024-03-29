using SimpleNotes.ApiTypes;

namespace SimpleNotes.Abstract;

public interface INoteRepository
{
    Task<DetailedNoteVm> GetAsync(Guid userId, Guid noteId);
    Task<IReadOnlyList<ListNoteVm>> GetAllForUserAsync(Guid userId, OrderColumn? orderColumn = null, bool isDesc = false);
    Task AddAsync(Guid userId, CreateNoteDto createNoteDto);
    Task EditAsync(Guid userId, Guid noteId, EditNoteDto editNoteDto);
    Task RemoveAsync(Guid userId, Guid noteId);
}
