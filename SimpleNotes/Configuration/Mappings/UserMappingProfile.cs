using AutoMapper;
using SimpleNotes.Abstract;
using SimpleNotes.Dtos;
using SimpleNotes.Models.User;

namespace SimpleNotes.Configuration.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile(IDateTimeProvider dateTimeProvider)
    {
        CreateMap<RegisterDto, User>()
            .ForMember(user => user.Id, opt => opt.Ignore())
            .ForMember(user => user.NickName, opt => opt.MapFrom(dto => dto.NickName))
            .ForMember(user => user.Email, opt => opt.MapFrom(dto => dto.Email))
            .ForMember(user => user.Password, opt => opt.Ignore())
            .ForMember(user => user.CreateDateTimeUtc, opt => opt.MapFrom(_ => dateTimeProvider.UtcNow));
    }
}