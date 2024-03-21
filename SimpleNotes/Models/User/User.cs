namespace SimpleNotes.Models.User;

public class User
{
    public Guid Id { get; set; }
    public string NickName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public byte[] Password { get; set; } = null!;
    public DateTime CreateDateTimeUtc { get; set; }
}