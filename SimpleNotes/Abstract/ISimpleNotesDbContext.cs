using Microsoft.EntityFrameworkCore;
using SimpleNotes.Models.Note;
using SimpleNotes.Models.User;

namespace SimpleNotes.Abstract;

public interface ISimpleNotesDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Note> Notes { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}