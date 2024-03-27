using System.Net;

namespace SimpleNotes.Errors;

public class ArgumentsNotFoundInRequestException : Exception, IServiceException
{
    public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
    public string ErrorMessage => "Arguments not found in request.";
}