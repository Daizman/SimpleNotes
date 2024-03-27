using System.Net;

namespace SimpleNotes.Errors;

public class InvalidArgumentsException : Exception, IServiceException
{
    public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
    public string ErrorMessage => "Invalid arguments.";
}