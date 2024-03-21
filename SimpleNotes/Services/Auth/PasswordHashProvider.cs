using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using SimpleNotes.Abstract;

namespace SimpleNotes.Services.Auth;

public class PasswordHashProvider(IOptions<Settings.PasswordHashProvider> settings) : IPasswordHashProvider
{
    private readonly Settings.PasswordHashProvider _settings = settings.Value;
    private const int Iterations = 1000;
    private const int KeySize = 128;
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.MD5;

    public byte[] GetHash(string password)
    {
        var saltBytes = Encoding.UTF8.GetBytes(_settings.Salt);
        var hashed = Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, Iterations, Algorithm, KeySize);

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