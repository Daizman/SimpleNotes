namespace SimpleNotes.Endpoints;

public static class UseSimpleNotesEndpointsExtension
{
    public static WebApplication UseSimpleNotesEndpoints(this WebApplication app)
    {
        app.MapSimpleNotesHealthChecks();
        app.MapNoteEndpoints();
        app.MapAuthenticationEndpoints();

        return app;
    }
}