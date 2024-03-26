using SimpleNotes.Abstract;
using SimpleNotes.Dtos;

namespace SimpleNotes.Services.Auth;

public class AuthService(
    IUserRepository userRepository, 
    IPasswordHashProvider passwordHashProvider,
    IJwtTokenGenerator jwtTokenGenerator) : IAuthService
{
    public async Task<AuthenticationResult> LoginAsync(LoginDto loginDto)
    {
        var user = await userRepository.GetUserAsync(loginDto.NickName);
        if (!passwordHashProvider.Verify(loginDto.Password, user.Password))
        {
            throw new Exception("Incorrect password");
        }
        
        var token = jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(new AuthenticatedUser(user.Id, user.NickName, token));
    }

    public async Task<AuthenticationResult> RegisterAsync(RegisterDto registerDto)
    {
        if (await userRepository.IsUserExistsAsync(registerDto.NickName))
        {
            throw new Exception("User already exists");
        }

        await userRepository.AddUserAsync(registerDto);
        var user = await userRepository.GetUserAsync(registerDto.NickName);
        var token = jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(new AuthenticatedUser(user.Id, user.NickName, token));
    }

    public Task LogoutAsync(string token)
    {
        // тут скорее всего нужно создать хранилище токенов и удалять из него токен при логауте, а в аутентификации
        // проверять, что токен есть в хранилище
        throw new NotImplementedException();
    }
}