using SimpleNotes.Abstract;
using SimpleNotes.Dtos;

namespace SimpleNotes.Services.Auth;

public class AuthService : IAuthService
{
    public Task<AuthenticationResult> LoginAsync(LoginDto loginDto)
    {
        throw new NotImplementedException();
    }

    public Task<AuthenticationResult> RegisterAsync(RegisterDto registerDto)
    {
        throw new NotImplementedException();
    }

    public Task LogoutAsync(string token)
    {
        throw new NotImplementedException();
    }
}