using AutoMapper;
using Financial.Chat.Domain.Core.Commands.Message;
using Financial.Chat.Domain.Core.Entity;
using System;

namespace Financial.Chat.Application.AutoMapper.Mappers
{
    public class MessageMapper : Profile
    {
        public MessageMapper()
        {
            CreateMap<MessageAddCommand, Messages>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
