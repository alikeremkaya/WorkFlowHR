using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowHR.Domain.Entities;
using WorkFlowHR.Infrastructure.AppContext;
using WorkFlowHR.Infrastructure.DataAccess.BaseRepository;

namespace WorkFlowHR.Infrastructure.Repositories.ManagerRepositories
{
    public class ManagerRepository : EFBaseRepository<Manager>, IManagerRepository
    {
        public ManagerRepository(AppDbContext context) : base(context)
        {

        }

        /// <summary>
        ///   string tipinde bir IdentityId alır ve bu IdentityId'ye sahip olan Manager nesnesini bulur.
        /// </summary>
        /// <param name="identityId">string tipinde bir (identityId) alır</param>
        /// <returns> Eğer veritabanında bu IdentityId'ye sahip bir Manager nesnesi yoksa, null değeri döndürür.</returns>
        /// 

        public Task<Manager?> GetByIdentityId(string identityId)
        {
            return _table.FirstOrDefaultAsync(x => x.IdentityId == identityId);

        }
    }
}
