using FluentValidation;
using SimpleNotes.ApiTypes;
using SimpleNotes.Errors;

namespace SimpleNotes.Filters;

public class CreateNoteDtoValidationFilter(IValidator<CreateNoteDto> validator) : IEndpointFilter
{
    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var note =
            context.Arguments.FirstOrDefault(obj => obj is not null && obj.GetType() == typeof(CreateNoteDto)) as
                CreateNoteDto;

        if (note is null)
        {
            throw new ArgumentsNotFoundInRequestException();
        }

        var validationResult = validator.Validate(note);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        return next(context);
    }
}