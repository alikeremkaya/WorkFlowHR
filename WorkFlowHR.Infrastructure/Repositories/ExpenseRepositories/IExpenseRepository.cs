
using WorkFlowHR.Domain.Entities;
using WorkFlowHR.Infrastructure.DataAccess.Interfaces;

namespace WorkFlowHR.Infrastructure.Repositories.ExpenseRepositories
{
    public interface IExpenseRepository :
        IAsyncRepository,
        IAsyncInsertableRepository<Expense>,
        IAsyncFindableRepository<Expense>,
        IAsyncQueryableRepository<Expense>,
        IAsyncUpdatableRepository<Expense>,
        IAsyncDeletableRepository<Expense>
    {
    }
}
