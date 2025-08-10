
using WorkFlowHR.Domain.Entities;
using WorkFlowHR.Infrastructure.AppContext;
using WorkFlowHR.Infrastructure.DataAccess.BaseRepository;

namespace WorkFlowHR.Infrastructure.Repositories.ExpenseRepositories
{
    internal class ExpenseRepository : EFBaseRepository<Expense>, IExpenseRepository
    {
        public ExpenseRepository(AppDbContext context) : base(context) { }
    }
    
    
}
