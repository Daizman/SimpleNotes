using Microsoft.EntityFrameworkCore;
using SimpleNotes.Abstract;
using SimpleNotes.Database.Configurations;
using SimpleNotes.Models.Note;

namespace SimpleNotes.Database;

public class SimpleNotesDbContext : DbContext, ISimpleNotesDbContext
{
    public DbSet<Note> Notes { get; set; }
    
    public SimpleNotesDbContext(DbContextOptions<SimpleNotesDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new NoteConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}