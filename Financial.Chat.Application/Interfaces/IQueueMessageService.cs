using Financial.Chat.Domain.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Chat.Application.Interfaces
{
    public interface IQueueMessageService
    {
        Task SendMessageAsync(MessageDto messageDto);
    }
}
