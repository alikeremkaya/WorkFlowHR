using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkFlowHR.Infrastructure.AppContext;
using WorkFlowHR.Infrastructure.Repositories.AdminRepositories;


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
            services.AddScoped<IAdminRepository, AdminRepository>();
            //services.AddScoped<IManagerRepository, ManagerRepository>();
           

            //AdminSeed.SeedAsync(configuration).GetAwaiter().GetResult();
            return services;
        }
    }
}
