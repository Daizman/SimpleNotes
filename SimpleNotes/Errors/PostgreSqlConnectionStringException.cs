namespace SimpleNotes.Errors;

public class PostgreSqlConnectionStringException : Exception
{
    public PostgreSqlConnectionStringException() : base("PostgreSQL connection not found in appsettings.json.")
    {
    }
}