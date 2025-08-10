

using WorkFlowHR.Domain.Entities;
using WorkFlowHR.Infrastructure.AppContext;
using WorkFlowHR.Infrastructure.DataAccess.BaseRepository;

namespace WorkFlowHR.Infrastructure.Repositories.AppUserRepositories;

public class AppUserRepository : EFBaseRepository<AppUser>, IAppUserRepository
{
    public AppUserRepository(AppDbContext context) : base(context) { }
    
        
    
}
