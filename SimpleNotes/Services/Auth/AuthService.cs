using Microsoft.Extensions.Caching.Memory;
using SimpleNotes.Abstract;
using SimpleNotes.ApiTypes;
using SimpleNotes.Errors;

namespace SimpleNotes.Services.Auth;

public class AuthService(
    IUserRepository userRepository, 
    IPasswordHashProvider passwordHashProvider,
    IJwtTokenGenerator jwtTokenGenerator,
    IMemoryCache memoryCache) : IAuthService
{
    public async Task<AuthenticationResult> LoginAsync(LoginDto loginDto)
    {
        var user = await userRepository.GetUserAsync(loginDto.NickName);
        if (!passwordHashProvider.Verify(loginDto.Password, user.Password))
        {
            throw new InvalidArgumentsException();
        }
        
        var token = jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(new AuthenticatedUser(user.Id, user.NickName, token));
    }

    public async Task<AuthenticationResult> RegisterAsync(RegisterDto registerDto)
    {
        if (await userRepository.IsUserExistsAsync(registerDto.NickName))
        {
            throw new UserAlreadyExistsException();
        }

        await userRepository.AddUserAsync(registerDto);
        var user = await userRepository.GetUserAsync(registerDto.NickName);
        var token = jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(new AuthenticatedUser(user.Id, user.NickName, token));
    }

    public void Logout(Guid userId)
    {
        memoryCache.Remove(userId);
    }
}