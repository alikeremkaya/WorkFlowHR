using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkFlowHR.Application.Services.AdvanceServices;
using WorkFlowHR.Application.Services.ExpenseServices;
using WorkFlowHR.Application.Services.LeaveServices;
using WorkFlowHR.UI.Areas.Manager.Models.SummaryVMs;

namespace WorkFlowHR.UI.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Policy = "ManagerOnly")]
    public class SummaryController : Controller
    {
        private readonly IAdvanceService _advanceService;
        private readonly IExpenseService _expenseService;
        private readonly ILeaveService _leaveService;

        public SummaryController(IAdvanceService advanceService, IExpenseService expenseService, ILeaveService leaveService)
        {
            _advanceService = advanceService;
            _expenseService = expenseService;
            _leaveService = leaveService;
        }

        public async Task<IActionResult> Index(Guid? employeeId)
        {
            var advances = await _advanceService.GetAllAsync();
            var expenses = await _expenseService.GetAllAsync();
            var leaves = await _leaveService.GetAllAsync();

            var model = new SummaryViewModel
            {
                Advances = employeeId.HasValue ? advances.Data.Where(a => a.AppUserId == employeeId.Value).ToList() : advances.Data,
                Expenses = employeeId.HasValue ? expenses.Data.Where(e => e.AppUserId == employeeId.Value).ToList() : expenses.Data,
                Leaves = employeeId.HasValue ? leaves.Data.Where(l => l.AppUserId == employeeId.Value).ToList() : leaves.Data
            };

            return View(model);
        }
    }
}
