using FluentValidation;
using SimpleNotes.Dtos;
using SimpleNotes.Models.Note;

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
        RuleFor(note => note.Priority)
            .IsEnumName(typeof(Priority), caseSensitive: false);
    }
}