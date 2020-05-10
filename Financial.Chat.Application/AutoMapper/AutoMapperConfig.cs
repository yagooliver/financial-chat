using AutoMapper;
using Financial.Chat.Application.AutoMapper.Mappers;

namespace Financial.Chat.Application.AutoMapper
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserMapper());
            });
        }
    }
}
