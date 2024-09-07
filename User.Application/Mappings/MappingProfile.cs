using AutoMapper;
using User.Application.Dtos;
using User.Application.Services.User.Commands.CreateUser;
using User.Domain.Entities;

namespace User.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Dtos
            CreateMap<UserObj, UserDto>().ReverseMap();
            CreateMap<Person, PersonDto>().ReverseMap();

            //Commands

            // Mapping from CreateUserCommand to Person
            CreateMap<CreateUserCommand, Person>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.PersonName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.PersonEmail));

            CreateMap<CreateUserCommand, UserObj>()
                     .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.UserPassword))
                     .ForPath(dest => dest.Person!.Name, opt => opt.MapFrom(src => src.PersonName))
                     .ForPath(dest => dest.Person!.Email, opt => opt.MapFrom(src => src.PersonEmail));

        }
    }
}
