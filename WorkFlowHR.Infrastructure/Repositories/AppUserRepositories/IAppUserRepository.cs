
using WorkFlowHR.Domain.Entities;
using WorkFlowHR.Infrastructure.DataAccess.Interfaces;

namespace WorkFlowHR.Infrastructure.Repositories.AppUserRepositories;

public interface IAppUserRepository : IAsyncRepository, IAsyncInsertableRepository<AppUser>, IAsyncFindableRepository<AppUser>,
    IAsyncQueryableRepository<AppUser>, IAsyncUpdatableRepository<AppUser>, IAsyncDeletableRepository<AppUser>, IAsyncOrderableRepository<AppUser>
{
}
