using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using WorkFlowHR.UI.Models;

namespace WorkFlowHR.UI.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Root�a gelen kullan�c�y� rol�ne g�re Manager veya Employee area�s�na y�nlendirir
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // OpenID Connect challenge: kullan�c�y� Microsoft�a y�nlendir
                return Challenge(
                    new AuthenticationProperties { RedirectUri = "/" },
                    OpenIdConnectDefaults.AuthenticationScheme);
            }
            var email = User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
            var managerEmails = new[] { "ali.kaya@pablika.com" };

            if (managerEmails.Contains(email, StringComparer.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index", "Home", new { area = "Manager" });
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "Employee" });
            }


        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
