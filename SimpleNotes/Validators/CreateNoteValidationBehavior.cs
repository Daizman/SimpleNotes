using FluentValidation;
using SimpleNotes.Dtos;

namespace SimpleNotes.Validators;

public class CreateNoteValidationBehavior : AbstractValidator<CreateNoteDto>
{
    public CreateNoteValidationBehavior()
    {
        RuleFor(note => note.Title)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(note => note.Description)
            .MaximumLength(500);
    }
}