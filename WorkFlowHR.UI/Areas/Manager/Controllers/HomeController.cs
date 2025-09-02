using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorkFlowHR.Application.Services.AdvanceServices;
using WorkFlowHR.Application.Services.AppUserServices;
using WorkFlowHR.Application.Services.ExpenseServices;
using WorkFlowHR.Application.Services.LeaveServices;
using WorkFlowHR.Application.Services.LeaveTypeServices;
using WorkFlowHR.Domain.Enums;
using WorkFlowHR.UI.Areas.Manager.Models.AdvanceVMs;
using WorkFlowHR.UI.Areas.Manager.Models.ExpenseVMs;
using WorkFlowHR.UI.Areas.Manager.Models.LeaveVMs;
using WorkFlowHR.UI.Areas.Manager.Models.ManagerVMs;

namespace WorkFlowHR.UI.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Policy = "ManagerOnly")]
    public class HomeController : Controller
    {
        private readonly IAppUserService _appUserService;
        private readonly IAdvanceService _advanceService;
        private readonly ILeaveService _leaveService;
        private readonly IExpenseService _expenseService;

        public HomeController(
            IAppUserService appUserService,
            IAdvanceService advanceService,
            ILeaveService leaveService,
            IExpenseService expenseService)
        {
            _appUserService = appUserService;
            _advanceService = advanceService;
            _leaveService = leaveService;
            _expenseService = expenseService;
        }

        public async Task<IActionResult> Index()
        {
            var email =
        User.FindFirstValue(ClaimTypes.Email) ??
        User.FindFirstValue("preferred_username") ??
        User.FindFirst("emails")?.Value ?? string.Empty;

          
            string loginFirstName =
                User.FindFirstValue(ClaimTypes.GivenName)
                ?? (User.Identity?.Name?.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault())
                ?? (!string.IsNullOrWhiteSpace(email) ? email.Split('@')[0] : "User");

            ViewBag.LoginFirstName = loginFirstName;
            ViewBag.Email = email;

            var roleFromClaim = User.FindFirstValue(ClaimTypes.Role);
            ViewBag.Role = !string.IsNullOrWhiteSpace(roleFromClaim)
                ? roleFromClaim
                : (User.IsInRole("Manager") ? "Manager"
                  : User.IsInRole("Employee") ? "Employee"
                  : "User");



            var allUsersResult = await _appUserService.GetAllAsync();
            var employeesCount = 0;
            if (allUsersResult.IsSuccess && allUsersResult.Data is not null)
            {
                employeesCount = allUsersResult.Data.Count(u =>
                    string.Equals(u.Role, nameof(Roles.Employee), StringComparison.OrdinalIgnoreCase));
            }
            ViewBag.EmployeesCount = employeesCount;

            var advancesResult = await _advanceService.GetAllAsync();
            var advanceVMs = advancesResult.IsSuccess && advancesResult.Data is not null
                ? advancesResult.Data.Adapt<List<AdvanceListVM>>()
                : new List<AdvanceListVM>();

            var leavesResult = await _leaveService.GetAllAsync();
            var leaveVMs = leavesResult.IsSuccess && leavesResult.Data is not null
                ? leavesResult.Data.Adapt<List<LeaveListVM>>()
                : new List<LeaveListVM>();

            var expensesResult = await _expenseService.GetAllAsync();
            var expenseVMs = expensesResult.IsSuccess && expensesResult.Data is not null
                ? expensesResult.Data.Adapt<List<ExpenseListVM>>()
                : new List<ExpenseListVM>();

            ViewBag.ExpenseCounts = BuildDayOfWeekCounts(expenseVMs.GroupBy(l => l.ExpenseDate.DayOfWeek));
            ViewBag.AdvanceCounts = BuildDayOfWeekCounts(advanceVMs.GroupBy(l => l.AdvanceDate.DayOfWeek));
            ViewBag.LeaveCounts = BuildDayOfWeekCounts(leaveVMs.GroupBy(l => l.StartDate.DayOfWeek));

            ViewBag.TotalLeaves = leaveVMs.Count;
            ViewBag.TotalAdvances = advanceVMs.Count;

            return View();
        }

        private static Dictionary<string, int> BuildDayOfWeekCounts(
            IEnumerable<IGrouping<DayOfWeek, object>> groups)
        {
            var dict = new Dictionary<string, int>
            {
                { "Monday", 0 }, { "Tuesday", 0 }, { "Wednesday", 0 },
                { "Thursday", 0 }, { "Friday", 0 }, { "Saturday", 0 }, { "Sunday", 0 }
            };

            foreach (var g in groups)
                dict[g.Key.ToString()] = g.Count();

            return dict;
        }
    }
}

