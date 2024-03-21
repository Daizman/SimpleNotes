using SimpleNotes.Models.User;

namespace SimpleNotes.Dtos;

public record RegisterDto(string NickName, string Email, string Password);

public record LoginDto(string NickName, string Password);

public record AuthenticationResult(User User, string Token);
