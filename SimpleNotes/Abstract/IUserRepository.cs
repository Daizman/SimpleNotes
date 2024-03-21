using SimpleNotes.Dtos;

namespace SimpleNotes.Abstract;

public interface IUserRepository
{
    Task<bool> IsUserExistsAsync(string nickName);
    Task AddUserAsync(RegisterDto registerDtoDto);
}