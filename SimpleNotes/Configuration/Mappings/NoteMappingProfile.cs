using AutoMapper;
using SimpleNotes.Abstract;
using SimpleNotes.Dtos;
using SimpleNotes.Models.Note;

namespace SimpleNotes.Configuration.Mappings;

public class NoteMappingProfile : Profile
{
    public NoteMappingProfile(IDateTimeProvider dateTimeProvider)
    {
        CreateMap<Note, DetailedNoteVm>()
            .ForCtorParam(nameof(DetailedNoteVm.Id), opt => opt.MapFrom(note => note.Id))
            .ForCtorParam(nameof(DetailedNoteVm.Title), opt => opt.MapFrom(note => note.Title))
            .ForCtorParam(nameof(DetailedNoteVm.Description), opt => opt.MapFrom(note => note.Description))
            .ForCtorParam(nameof(DetailedNoteVm.IsCompleted), opt => opt.MapFrom(note => note.IsCompleted))
            .ForCtorParam(nameof(DetailedNoteVm.Priority), opt => opt.MapFrom(note => note.Priority.ToString()));
        CreateMap<Note, ListNoteVm>()
            .ForCtorParam(nameof(ListNoteVm.Id), opt => opt.MapFrom(note => note.Id))
            .ForCtorParam(nameof(ListNoteVm.Title), opt => opt.MapFrom(note => note.Title))
            .ForCtorParam(nameof(ListNoteVm.IsCompleted), opt => opt.MapFrom(note => note.IsCompleted))
            .ForCtorParam(nameof(ListNoteVm.Priority), opt => opt.MapFrom(note => note.Priority.ToString()));
        
        CreateMap<(Guid UserId, CreateNoteDto CreateNoteDto), Note>()
            .ConvertUsing(src => new Note
            {
                Id = Guid.NewGuid(),
                Title = src.CreateNoteDto.Title,
                Description = src.CreateNoteDto.Description,
                CreationDateTime = dateTimeProvider.UtcNow,
                UpdateDateTime = dateTimeProvider.UtcNow,
                IsCompleted = src.CreateNoteDto.IsCompleted,
                Priority = StringToPriorityConvertor(src.CreateNoteDto.Priority),
                UserId = src.UserId,
            });
        CreateMap<(Guid UserId, Guid NoteId, EditNoteDto EditNoteDto), Note>()
            .ConvertUsing(src => new Note
            {
                Id = src.NoteId,
                Title = src.EditNoteDto.Title,
                Description = src.EditNoteDto.Description,
                UpdateDateTime = dateTimeProvider.UtcNow,
                IsCompleted = src.EditNoteDto.IsCompleted,
                Priority = StringToPriorityConvertor(src.EditNoteDto.Priority),
                UserId = src.UserId,
            });
    }

    private static Priority StringToPriorityConvertor(string priorityStr) => 
        Enum.TryParse<Priority>(priorityStr, true, out var priority)
            ? priority
            : default;
}