using System.Net;

namespace SimpleNotes.Errors;

public class UserAlreadyExistsException : Exception, IServiceException
{
    public HttpStatusCode StatusCode => HttpStatusCode.Conflict;
    public string ErrorMessage => "User already exists.";
}