using System.Net;

namespace SimpleNotes.Errors;

public class NoteNotFoundException : Exception, IServiceException
{
    public HttpStatusCode StatusCode => HttpStatusCode.NotFound;
    public string ErrorMessage => "Note not found.";
}