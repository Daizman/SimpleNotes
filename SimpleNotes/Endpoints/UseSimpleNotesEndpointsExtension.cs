namespace SimpleNotes.Endpoints;

public static class UseSimpleNotesEndpointsExtension
{
    public static WebApplication UseSimpleNotesEndpoints(this WebApplication app)
    {
        app.MapNoteEndpoints();
        app.MapAuthenticationEndpoints();

        return app;
    }
}