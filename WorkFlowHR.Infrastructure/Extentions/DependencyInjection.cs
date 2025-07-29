using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowHR.Infrastructure.Repositories.AdminRepositories;
using WorkFlowHR.Infrastructure.Repositories.ManagerRepositories;

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
