using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowHR.Domain.Entities;
using WorkFlowHR.Infrastructure.DataAccess.Interfaces;

namespace WorkFlowHR.Infrastructure.Repositories.AdminRepositories
{
    public interface IAdminRepository : IAsyncRepository, IAsyncInsertableRepository<Admin>, IAsyncFindableRepository<Admin>,
        IAsyncQueryableRepository<Admin>, IAsyncUpdatableRepository<Admin>, IAsyncDeletableRepository<Admin>, IAsyncTransactionRepository
    {
        Task<Admin?> GetByIdentityId(string identityId);

        //  string tipinde bir IdentityId alır ve bu IdentityId'ye sahip olan Admin nesnesini bulur.
    }
}
