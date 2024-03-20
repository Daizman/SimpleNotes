namespace SimpleNotes.Settings;

public class PostgreSqlConnection
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public string Database { get; set; }
    public string ConnectionString => $"host={Host};port={Port};username={User};password={Password};database={Database}";
}