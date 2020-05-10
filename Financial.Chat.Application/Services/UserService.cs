using AutoMapper;
using Financial.Chat.Application.AutoMapper.Mappers;
using Financial.Chat.Domain.Core.Entity;
using Financial.Chat.Domain.Core.Interfaces;
using Financial.Chat.Domain.Core.Interfaces.Services;
using Financial.Chat.Domain.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Financial.Chat.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public List<Messages> GetMessages() => _userRepository.GetMessages().OrderByDescending(x => x.Date).Take(50).ToList();

        public List<Messages> GetMessages(string email) => _userRepository.GetMessages().Where(x => x.Consumer == email || x.Sender == email).OrderByDescending(x => x.Date).Take(50).ToList();

        public UserDto GetUser(Guid id)
        {
            var user = _userRepository.GetById(id);
            var userMapped = _mapper.Map<UserDto>(user);

            return userMapped;
        }

        public List<UserDto> GetUsers()
        {
            var users = _userRepository.GetAll().ToList();
            var usersMapped = _mapper.Map<List<UserDto>>(users);

            return usersMapped;
        }
    }
}
