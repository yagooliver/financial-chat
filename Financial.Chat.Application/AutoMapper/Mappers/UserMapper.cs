using AutoMapper;
using Financial.Chat.Domain.Core.Commands;
using Financial.Chat.Domain.Core.Entity;
using Financial.Chat.Domain.Shared.Entity;

namespace Financial.Chat.Application.AutoMapper.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<UserAddCommand, User>();
            CreateMap<User, UserDto>();
        }
    }
}
