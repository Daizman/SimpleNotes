namespace SimpleNotes.Dtos;

public record RegisterDto(string NickName, string Email, string Password);

public record LoginDto(string NickName, string Password);

public record AuthenticatedUser(Guid UserId, string NickName, string Token);

public record AuthenticationResult(AuthenticatedUser User);
