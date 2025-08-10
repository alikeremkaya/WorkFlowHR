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
            // 1) Identity (Cookie + Token)
            

            // 2) Dil / yerelleştirme
            services.AddLocalization(opts => opts.ResourcesPath = "Resources");

            services.AddControllersWithViews(opts =>
                     opts.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
                    .AddMicrosoftIdentityUI()                                   // ↖ login / logout Razor Pages
                    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                    .AddDataAnnotationsLocalization();

            services.AddRazorPages(); // MicrosoftIdentity UI Razor Pages

            services.Configure<RequestLocalizationOptions>(opts =>
            {
                var cultures = new[] { new CultureInfo("tr"), new CultureInfo("en") };
                opts.DefaultRequestCulture = new RequestCulture("tr");
                opts.SupportedUICultures = cultures;
                opts.SupportedCultures = cultures;
            });

            // 3) FluentValidation (UI katmanı validator’ları bu assembly’deyse)
            services.AddFluentValidationAutoValidation()
                    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
