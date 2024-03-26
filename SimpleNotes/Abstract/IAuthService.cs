using SimpleNotes.Dtos;

namespace SimpleNotes.Abstract;

public interface IAuthService
{
    Task<AuthenticationResult> LoginAsync(LoginDto loginDto);

    Task<AuthenticationResult> RegisterAsync(RegisterDto registerDto);

    Task LogoutAsync(string token);
}