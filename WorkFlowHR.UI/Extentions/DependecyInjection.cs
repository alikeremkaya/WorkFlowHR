using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Identity.Web.UI;
using System.Globalization;
using System.Reflection;
using WorkFlowHR.Infrastructure.AppContext;

namespace WorkFlowHR.UI.Extentions
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddUIServices(this IServiceCollection services)
        {
           
            

           
            services.AddLocalization(opts => opts.ResourcesPath = "Resources");

            services.AddControllersWithViews(opts =>
                     opts.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
                    .AddMicrosoftIdentityUI()                                  
                    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                    .AddDataAnnotationsLocalization();

            services.AddRazorPages(); 

            services.Configure<RequestLocalizationOptions>(opts =>
            {
                var cultures = new[] { new CultureInfo("tr"), new CultureInfo("en") };
                opts.DefaultRequestCulture = new RequestCulture("tr");
                opts.SupportedUICultures = cultures;
                opts.SupportedCultures = cultures;
            });

           
            services.AddFluentValidationAutoValidation()
                    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
