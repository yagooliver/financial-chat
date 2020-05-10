using Financial.Chat.Domain.Shared.Entity;
using System;

namespace Financial.Chat.Domain.Core.Entity
{
    public class Messages : EntityBase
    {
        public string Message { get;set; }
        public string Sender { get; set; }
        public string Consumer { get; set; }
        public DateTime Date { get; set; }
    }
}
