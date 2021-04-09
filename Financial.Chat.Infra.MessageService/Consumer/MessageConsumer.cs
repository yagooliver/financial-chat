//using Financial.Chat.Domain.Shared.Entity;
//using MassTransit;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace Financial.Chat.Infra.MessageService
//{
//    public class MessageConsumer : IConsumer<MessageDto>
//    {
//        private readonly ILogger<MessageConsumer> _logger;

//        public MessageConsumer(ILogger<MessageConsumer> logger)
//        {
//            _logger = logger;
//        }

//        public async Task Consume(ConsumeContext<MessageDto> context)
//        {
//            await Task.CompletedTask;
//        }
//    }
//}
