using System.Net;

namespace SimpleNotes.Errors;

public interface IServiceException
{ 
    HttpStatusCode StatusCode { get; }
    string ErrorMessage { get; }
}