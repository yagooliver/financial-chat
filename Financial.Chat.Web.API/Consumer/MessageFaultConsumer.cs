using Financial.Chat.Domain.Shared.Entity;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Financial.Chat.Web.API.Consumer
{
    public class MessageFaultConsumer : IConsumer<Fault<MessageDto>>
    {
        public async Task Consume(ConsumeContext<Fault<MessageDto>> context)
        {
            await Task.CompletedTask;
        }
    }
}
