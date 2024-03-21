using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SimpleNotes.Abstract;
using SimpleNotes.Dtos;
using SimpleNotes.Models.User;

namespace SimpleNotes.Repositories;

public class UserRepository(
    ILogger<UserRepository> logger, 
    ISimpleNotesDbContext simpleNotesDbContext,
    IPasswordHashProvider passwordHashProvider,
    IMapper mapper) : IUserRepository
{
    public Task<bool> IsUserExistsAsync(string nickName)
        => simpleNotesDbContext.Users.AsNoTracking().AnyAsync(user => user.NickName == nickName);

    public async Task AddUserAsync(RegisterDto registerDtoDto)
    {
        if (await IsUserExistsAsync(registerDtoDto.NickName))
        {
            logger.LogWarning("User already exists");
            return;
        }

        var user = mapper.Map<User>(registerDtoDto);
        user.Password = passwordHashProvider.GetHash(registerDtoDto.Password);

        await simpleNotesDbContext.Users.AddAsync(user);
        await simpleNotesDbContext.SaveChangesAsync();
    }
}