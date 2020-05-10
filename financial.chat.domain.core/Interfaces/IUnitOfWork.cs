namespace Financial.Chat.Domain.Core.Interfaces
{
    public interface IUnitOfWork
    {
        bool Commit();
    }
}
