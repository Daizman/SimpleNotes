using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using SimpleNotes.Abstract;

namespace SimpleNotes.Services.Auth;

public class PasswordHashProvider(IOptions<Settings.PasswordHashProvider> settings) : IPasswordHashProvider
{
    private readonly Settings.PasswordHashProvider _settings = settings.Value;
    private const int IterationCount = 10000;
    private const int NumBytesRequested = 256;

    public byte[] GetHash(string password)
    {
        var saltBytes = Encoding.UTF8.GetBytes(_settings.Salt);
        var hashed = KeyDerivation.Pbkdf2(
            password,
            saltBytes, 
            KeyDerivationPrf.HMACSHA256, 
            IterationCount, 
            NumBytesRequested);

        return hashed;
    }

    public bool Verify(string password, byte[] expected)
    {
        var hashed = GetHash(password);
        if (hashed.Length != expected.Length)
            return false;
        for (int i = 0; i < hashed.Length; i++)
            if (hashed[i] != expected[i])
                return false;
        return true;
    }
}