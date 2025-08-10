using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowHR.Domain.Entities;
using WorkFlowHR.Infrastructure.AppContext;
using WorkFlowHR.Infrastructure.DataAccess.BaseRepository;

namespace WorkFlowHR.Infrastructure.Repositories.AdvanceRepositories
{
    public class AdvanceRepository : EFBaseRepository<Advance>, IAdvanceRepository 
    {
        public AdvanceRepository(AppDbContext ctx) : base(ctx) { }
    }
    
    
}
