using Financial.Chat.Domain.Core.Interfaces;
using Financial.Chat.Infra.Data.Context;

namespace Financial.Chat.Infra.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FinancialChatContext _financialChatContext;

        public UnitOfWork(FinancialChatContext financialChatContext)
        {
            _financialChatContext = financialChatContext;
        }

        public bool Commit() => _financialChatContext.SaveChanges() > 0;
    }
}
