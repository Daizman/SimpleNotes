using System.Net;

namespace SimpleNotes.Errors;

public class UserNotFoundException : Exception, IServiceException
{
    public HttpStatusCode StatusCode => HttpStatusCode.NotFound;
    public string ErrorMessage => "User not found.";
}