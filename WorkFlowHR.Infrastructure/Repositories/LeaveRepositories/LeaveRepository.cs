
using WorkFlowHR.Domain.Entities;
using WorkFlowHR.Infrastructure.AppContext;
using WorkFlowHR.Infrastructure.DataAccess.BaseRepository;

namespace WorkFlowHR.Infrastructure.Repositories.LeaveRepositories
{
    public class LeaveRepository : EFBaseRepository<Leave>, ILeaveRepository
    {
        public LeaveRepository(AppDbContext context) : base(context) { }
    }
}
