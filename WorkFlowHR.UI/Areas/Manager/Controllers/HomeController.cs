using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WorkFlowHR.UI.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Policy = "ManagerOnly")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Role = "Manager";
            return View();
        }
    }
}
