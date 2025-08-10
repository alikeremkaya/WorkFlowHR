
using WorkFlowHR.Domain.Entities;
using WorkFlowHR.Infrastructure.DataAccess.Interfaces;

namespace WorkFlowHR.Infrastructure.Repositories.LeaveTypeRepositories
{
    public interface ILeaveTypeRepository : IAsyncRepository, IAsyncInsertableRepository<LeaveType>, IAsyncFindableRepository<LeaveType>,
        IAsyncQueryableRepository<LeaveType>, IAsyncUpdatableRepository<LeaveType>, IAsyncDeletableRepository<LeaveType>, IAsyncTransactionRepository
    {
    }
}
