using AutoMapper;
using UserService.Application.Common.Dto;
using UserService.Application.Users.Commands;
using UserService.Domain.Entities;

namespace UserService.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>()
            .ReverseMap();

        CreateMap<User, CreateUserCommand>()
            .ReverseMap();
    }
}
