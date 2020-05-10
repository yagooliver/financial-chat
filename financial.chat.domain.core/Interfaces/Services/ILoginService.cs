using Financial.Chat.Domain.Core.Entity;
using Financial.Chat.Domain.Shared.Entity;
using System;

namespace Financial.Chat.Domain.Core.Interfaces.Services
{
    public interface ILoginService
    {
        User Authenticate(string email, string password);
        TokenJWT GetToken(Guid id, string email);
    }
}
