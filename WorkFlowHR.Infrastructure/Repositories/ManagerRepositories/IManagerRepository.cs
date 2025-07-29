using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowHR.Domain.Entities;
using WorkFlowHR.Infrastructure.DataAccess.Interfaces;

namespace WorkFlowHR.Infrastructure.Repositories.ManagerRepositories
{
    public interface IManagerRepository : IAsyncRepository, IAsyncInsertableRepository<Manager>, IAsyncFindableRepository<Manager>,
        IAsyncQueryableRepository<Manager>, IAsyncUpdatableRepository<Manager>, IAsyncDeletableRepository<Manager>, IAsyncTransactionRepository
    {
        Task<Manager?> GetByIdentityId(string identityId);

        //  string tipinde bir IdentityId alır ve bu IdentityId'ye sahip olan Manager nesnesini bulur.
    }
}
