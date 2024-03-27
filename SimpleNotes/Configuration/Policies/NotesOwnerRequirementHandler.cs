using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace SimpleNotes.Configuration.Policies;

public class NotesOwnerRequirementHandler: AuthorizationHandler<NotesOwnerRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        NotesOwnerRequirement requirement)
    {
        var parseRes = Guid.TryParse(context.Resource?.ToString(), out var userId);
        if (!parseRes)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim is null)
        {
            context.Fail();
            return Task.CompletedTask;
        }
        
        if (userIdClaim.Value != userId.ToString())
        {
            context.Fail();
            return Task.CompletedTask;
        }
        
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}