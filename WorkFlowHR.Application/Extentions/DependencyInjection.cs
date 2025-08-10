using Microsoft.Extensions.DependencyInjection;
using WorkFlowHR.Application.Services.AdvanceServices;
using WorkFlowHR.Application.Services.AppUserServices;
using WorkFlowHR.Application.Services.ExpenseServices;
using WorkFlowHR.Application.Services.LeaveServices;
using WorkFlowHR.Application.Services.LeaveTypeServices;
using WorkFlowHR.Application.Services.MailServices;

namespace WorkFlowHR.Application.Extentions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
         
            services.AddScoped<IMailService, MailService>();

            services.AddScoped<IAppUserService, AppUserService>();

            services.AddScoped<IAdvanceService, AdvanceService>();

            services.AddScoped<ILeaveService, LeaveService>();

            services.AddScoped<ILeaveTypeService, LeaveTypeService>();

            services.AddScoped<IExpenseService, ExpenseService>();



            return services;
        }
    }
}
