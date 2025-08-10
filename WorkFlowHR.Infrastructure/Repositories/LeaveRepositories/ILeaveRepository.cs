
using WorkFlowHR.Domain.Entities;
using WorkFlowHR.Infrastructure.DataAccess.Interfaces;

namespace WorkFlowHR.Infrastructure.Repositories.LeaveRepositories
{
    public interface ILeaveRepository :
        IAsyncRepository,
        IAsyncInsertableRepository<Leave>,
        IAsyncFindableRepository<Leave>,
        IAsyncQueryableRepository<Leave>,
        IAsyncUpdatableRepository<Leave>,
        IAsyncDeletableRepository<Leave>,
        IAsyncOrderableRepository<Leave>
    {
    }
}
