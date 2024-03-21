using SimpleNotes.Dtos;

namespace SimpleNotes.Abstract;

// toDo: реализовать аутентификацию, авторизацию, логаут
public interface IAuthService
{
    Task<AuthenticationResult> LoginAsync(LoginDto loginDto);

    Task<AuthenticationResult> RegisterAsync(RegisterDto registerDto);

    Task LogoutAsync(string token);
}