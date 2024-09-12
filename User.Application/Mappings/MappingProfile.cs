using AutoMapper;
using User.Application.Services.User.Commands.CreateUser;
using User.Application.Services.User.Queries.ViewModel;
using User.Domain.Entities;

namespace User.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            //Commands

            // Mapping from CreateUserCommand to Person and User
            CreateMap<CreateUserCommand, Person>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.PersonName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.PersonEmail));

            CreateMap<CreateUserCommand, UserObj>()
                     .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.UserPassword))
                     .ForPath(dest => dest.Person!.Name, opt => opt.MapFrom(src => src.PersonName))
                     .ForPath(dest => dest.Person!.Email, opt => opt.MapFrom(src => src.PersonEmail));

            // Mapping from UserViewModel to Person and User
            CreateMap<UserViewModel, UserObj>()
                   .ForMember(dest => dest.GuidId, opt => opt.MapFrom(src => src.GuidId))
                   .ForPath(dest => dest.Person!.Name, opt => opt.MapFrom(src => src.PersonName))
                   .ForPath(dest => dest.Person!.Email, opt => opt.MapFrom(src => src.PersonEmail))
                   .ForPath(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt)).ReverseMap();

        }
    }
}
