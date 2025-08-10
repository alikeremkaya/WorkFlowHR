using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WorkFlowHR.UI.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Policy = "EmployeeOnly")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Role = "Employee";
            return View();
        }
    }
}
