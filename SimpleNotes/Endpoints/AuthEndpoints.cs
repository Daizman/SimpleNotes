namespace SimpleNotes.Endpoints;

// toDo: добавить эндпоинты аутентификации
public static class AuthEndpoints
{
    public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
    {
        var authenticationApi = app.MapGroup("/api/auth");
    }
}