using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.EntityFrameworkCore;
using WorkFlowHR.Infrastructure.AppContext;
using WorkFlowHR.Application.Extentions;
using WorkFlowHR.Infrastructure.Extentions;
using WorkFlowHR.UI.Extentions;
using System.Security.Claims;
using Hangfire;
using WorkFlowHR.Domain.Entities;
using Mapster;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("AppConnectionDev"));

});
// 1) Uygulama & Altyapı servisleri
builder.Services.AddApplicationServices();
builder.Services.AddHttpContextAccessor();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddUIServices();

// 2) Authentication / Azure AD
builder.Services
    .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));


builder.Services.Configure<OpenIdConnectOptions>(
    OpenIdConnectDefaults.AuthenticationScheme, options =>
    {
        options.Events.OnTokenValidated = async ctx =>
        {
            var claims = ctx.Principal!.Claims;

            // 1) Zorunlu claim’leri al
            var oid = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (oid is null || email is null)
                return;

            // 2) Scoped servisleri al
            using var scope = ctx.HttpContext.RequestServices.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var cfg = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            // 3) Manager listesi
            var managers = cfg.GetSection("Authorization:ManagerEmails")
                              .Get<string[]>() ?? Array.Empty<string>();

            // 4) Mevcut kullanıcıyı bul
            var user = await db.AppUsers
                .FirstOrDefaultAsync(u => u.AzureAdObjectId == oid);

            // 5) E-posta prefix’ini alalım (ali.kaya@… ⇒ ali.kaya)
            var prefix = email.Split('@').First();

            if (user == null)
            {
                // Yeni kullanıcı
                user = new AppUser
                {
                    AzureAdObjectId = oid,
                    Email = email,
                    DisplayName = prefix,
                    FirstName = prefix,       // e-posta prefix’i
                    LastName = string.Empty, // boş bırakıyoruz
                    Role = managers.Contains(email, StringComparer.OrdinalIgnoreCase)
                                      ? "Manager"
                                      : "Employee"
                };
                db.AppUsers.Add(user);
            }
            else
            {
                // Güncelle
                user.Email = email;
                user.DisplayName = prefix;
                user.FirstName = prefix;
                user.LastName = string.Empty;
                user.Role = managers.Contains(email, StringComparer.OrdinalIgnoreCase)
                                  ? "Manager"
                                  : "Employee";
                db.AppUsers.Update(user);
            }

            await db.SaveChangesAsync();

            // 6) Role claim ekle
            var identity = (ClaimsIdentity)ctx.Principal.Identity!;
            identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));
        };
    });
// 3) Razor Pages (Microsoft Identity UI)

builder.Services.AddControllersWithViews()            // Sadece bir kez
       .AddMicrosoftIdentityUI();                      // Login/logout UI

// 4) Yetkilendirme politikaları
var managerEmails = builder.Configuration
    .GetSection("Authorization:ManagerEmails")
    .Get<string[]>() ?? Array.Empty<string>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ManagerOnly", policy => policy.RequireAssertion(ctx =>
    {
        var email = ctx.User.FindFirstValue(ClaimTypes.Email);
        return email is not null && managerEmails.Contains(email, StringComparer.OrdinalIgnoreCase);
    }));
    options.AddPolicy("EmployeeOnly", policy => policy.RequireAssertion(ctx =>
    {
        var email = ctx.User.FindFirstValue(ClaimTypes.Email);
        return email is not null && !managerEmails.Contains(email, StringComparer.OrdinalIgnoreCase);
    }));
});

// 5) OpenID Connect cookie ayarları (SameSite, correlation vs.)
builder.Services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.NonceCookie.SameSite = SameSiteMode.None;
    options.CorrelationCookie.SameSite = SameSiteMode.None;
    options.NonceCookie.SecurePolicy = CookieSecurePolicy.Always;
    options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;

    options.Events.OnAuthenticationFailed = ctx =>
    {
        var msg = Uri.EscapeDataString(ctx.Exception.Message);
        ctx.Response.Redirect($"/Home/Error?authError={msg}");
        ctx.HandleResponse();
        return Task.CompletedTask;
    };
});

// 6) Cookie policy
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
    options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
    options.Secure = CookieSecurePolicy.Always;
});





var app = builder.Build();

// 9) Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// 10) Routing
app.MapRazorPages();  // OIDC UI, AccessDenied vs.
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
