using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowHR.Domain.Entities;
using WorkFlowHR.Infrastructure.DataAccess.Interfaces;

namespace WorkFlowHR.Infrastructure.Repositories.AdvanceRepositories
{
    public interface IAdvanceRepository : IAsyncRepository, IAsyncInsertableRepository<Advance>, IAsyncFindableRepository<Advance>,
    IAsyncQueryableRepository<Advance>, IAsyncUpdatableRepository<Advance>, IAsyncDeletableRepository<Advance>, IAsyncOrderableRepository<Advance>
    {
    }
}
