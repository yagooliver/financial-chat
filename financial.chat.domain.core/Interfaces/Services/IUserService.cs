using Financial.Chat.Domain.Core.Entity;
using Financial.Chat.Domain.Shared.Entity;
using System;
using System.Collections.Generic;

namespace Financial.Chat.Domain.Core.Interfaces.Services
{
    public interface IUserService
    {
        List<UserDto> GetUsers();
        UserDto GetUser(Guid id);
        List<Messages> GetMessages();
        List<Messages> GetMessages(string email);
    }
}
