using Financial.Chat.Domain.Shared.Entity;

namespace Financial.Chat.Domain.Core.Entity
{
    public class User : EntityBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
