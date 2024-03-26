using Microsoft.AspNetCore.Mvc;
using SimpleNotes.Abstract;
using SimpleNotes.Dtos;

namespace SimpleNotes.Endpoints;

// toDo: добавить логаут
public static class AuthEndpoints
{
    public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
    {
        var authenticationApi = app.MapGroup("/auth");

        authenticationApi.MapPost("/register", async (
                [FromBody] RegisterDto registerDto, 
                IAuthService authService) =>
        {
            var result = await authService.RegisterAsync(registerDto);

            return Results.Ok(result);
        })
            .Produces<AuthenticationResult>();

        authenticationApi.MapPost("/login", async (
                [FromBody] LoginDto loginDto, 
                IAuthService authService) =>
        {
            var result = await authService.LoginAsync(loginDto);
            
            return Results.Ok(result);
        })
            .Produces<AuthenticationResult>();

        authenticationApi.MapDelete("/logout", () =>
        {

        });
    }
}