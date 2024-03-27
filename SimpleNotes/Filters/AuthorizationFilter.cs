using Microsoft.AspNetCore.Authorization;

namespace SimpleNotes.Filters;

public class AuthorizationFilter(IAuthorizationService authorizationService) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var userIdFromRequest = context.HttpContext.Request.RouteValues["userId"];
        var user = context.HttpContext.User;
        var authorizationResult = await authorizationService.AuthorizeAsync(user, userIdFromRequest, "NotesOwner");

        if (!authorizationResult.Succeeded)
        {
            return Results.Forbid();
        }

        return await next.Invoke(context);
    }
}