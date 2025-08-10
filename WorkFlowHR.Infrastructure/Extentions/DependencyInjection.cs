using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkFlowHR.Infrastructure.AppContext;
using WorkFlowHR.Infrastructure.Repositories.AdvanceRepositories;
using WorkFlowHR.Infrastructure.Repositories.AppUserRepositories;
using WorkFlowHR.Infrastructure.Repositories.ExpenseRepositories;
using WorkFlowHR.Infrastructure.Repositories.LeaveRepositories;
using WorkFlowHR.Infrastructure.Repositories.LeaveTypeRepositories;



namespace WorkFlowHR.Infrastructure.Extentions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(configuration.GetConnectionString("AppConnectionDev"));
               
            });

            services.AddScoped<IAppUserRepository, AppUserRepository>();
            services.AddScoped<IAdvanceRepository, AdvanceRepository>();
            services.AddScoped<ILeaveRepository, LeaveRepository>();
            services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();


            return services;
        }
    }
}
