using Financial.Chat.Domain.Shared.Entity;
using System;

namespace Financial.Chat.Domain.Core.Entity
{
    public class Messages : EntityBase
    {
        public Messages(string message, string sender, string consumer)
        {
            Id = Guid.NewGuid();
            Message = message;
            Sender = sender;
            Consumer = consumer;
            Date = DateTime.Now;
        }

        public string Message { get; private set; }
        public string Sender { get; private set; }
        public string Consumer { get; private set; }
        public DateTime Date { get; private set; }
    }
}
