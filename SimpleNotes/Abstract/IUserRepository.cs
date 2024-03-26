using SimpleNotes.Dtos;
using SimpleNotes.Models.User;

namespace SimpleNotes.Abstract;

public interface IUserRepository
{
    Task<bool> IsUserExistsAsync(string nickName);
    Task AddUserAsync(RegisterDto registerDtoDto);
    Task<User> GetUserAsync(string nickName);
}