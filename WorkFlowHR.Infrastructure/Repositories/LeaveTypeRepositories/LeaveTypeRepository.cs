
using WorkFlowHR.Domain.Entities;
using WorkFlowHR.Infrastructure.AppContext;
using WorkFlowHR.Infrastructure.DataAccess.BaseRepository;

namespace WorkFlowHR.Infrastructure.Repositories.LeaveTypeRepositories
{
    public class LeaveTypeRepository : EFBaseRepository<LeaveType>, ILeaveTypeRepository
    {
        public LeaveTypeRepository(AppDbContext context) : base(context) { }
    }
    
    
}
