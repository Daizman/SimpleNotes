using SimpleNotes.Models.User;

namespace SimpleNotes.Abstract;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}