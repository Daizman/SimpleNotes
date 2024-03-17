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
        // toDo: расписать конструкторы
        CreateMap<Note, ListNoteVm>();
        CreateMap<List<Note>, NotesVm>();

        CreateMap<(Guid userId, CreateNoteDto createNoteDto), Note>()
            .ForMember(note => note.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(note => note.CreationDateTime, opt => opt.MapFrom(_ => dateTimeProvider.Now))
            .ForMember(note => note.UpdateDateTime, opt => opt.MapFrom(_ => dateTimeProvider.Now))
            .ForMember(note => note.UserId, opt => opt.MapFrom((userId, _) => userId))
            .ForMember(note => note.Priority, opt => opt.MapFrom(x => ); // toDo: сделать каст
    }
}