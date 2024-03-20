namespace SimpleNotes.Database;

public static class DbInitializer
{
    public static async Task InitializeAsync(WebApplication app)
    {
        try
        {
            await using var scope = app.Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SimpleNotesDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
        }
        catch (Exception e)
        {
        }
    }
}