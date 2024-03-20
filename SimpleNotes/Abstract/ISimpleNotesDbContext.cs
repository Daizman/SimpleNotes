using Microsoft.EntityFrameworkCore;
using SimpleNotes.Models.Note;

namespace SimpleNotes.Abstract;

public interface ISimpleNotesDbContext
{
    DbSet<Note> Notes { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}