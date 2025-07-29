using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowHR.Application.Services.AccountServices;
using WorkFlowHR.Application.Services.AdminServices;
using WorkFlowHR.Application.Services.MailServices;
using WorkFlowHR.Application.Services.ManagerServices;

namespace WorkFlowHR.Application.Extentions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAdminService, AdminService>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IManagerService, ManagerService>();
            services.AddScoped<IMailService, MailService>();
       


            return services;
        }
    }
}
