using Financial.Chat.Domain.Core.Entity;
using Financial.Chat.Domain.Core.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Financial.Chat.Domain.Core.Interfaces
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        void Add(Messages messages);
        IEnumerable<Messages> GetMessages();
    }
}
