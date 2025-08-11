using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WorkFlowHR.Application.DTOs.LeaveDTOs;
using WorkFlowHR.Application.DTOs.LeaveTypeDTOs;
using WorkFlowHR.Application.Services.AppUserServices;
using WorkFlowHR.Application.Services.LeaveServices;
using WorkFlowHR.Application.Services.LeaveTypeServices;
using WorkFlowHR.UI.Areas.Manager.Models.LeaveVMs;

namespace WorkFlowHR.UI.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Policy = "EmployeeOnly")]
    public class LeaveController : Controller
    {

        private readonly ILeaveTypeService _leaveTypeService;
        private readonly ILeaveService _leaveService;
        private readonly IAppUserService _appUserService;
        private readonly ILogger<LeaveController> _logger;

        public LeaveController(
            ILeaveTypeService leaveTypeService,
            ILeaveService leaveService,
            IAppUserService appUserService,
            ILogger<LeaveController> logger)
        {
            _leaveTypeService = leaveTypeService;
            _leaveService = leaveService;
            _appUserService = appUserService;
            _logger = logger;
        }

        // LIST
        public async Task<IActionResult> Index()
        {
            var result = await _leaveService.GetAllAsync();
            if (!result.IsSuccess || result.Data == null)
            {
                await Console.Out.WriteLineAsync(result.Messages);
                return View(new List<LeaveListVM>());
            }

            var leaveVMs = result.Data.Adapt<List<LeaveListVM>>();
            return View(leaveVMs);
        }

        // DETAILS
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _leaveService.GetByIdAsync(id);
            if (!result.IsSuccess || result.Data == null)
            {
                await Console.Out.WriteLineAsync(result.Messages);
                return RedirectToAction(nameof(Index));
            }

            var vm = result.Data.Adapt<LeaveDetailsVM>();
            return View(vm);
        }

        // CREATE (GET)
        public async Task<IActionResult> Create()
        {
            var vm = new LeaveCreateVM
            {
                LeaveTypes = await GetLeaveTypes()
            };
            return View(vm);
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                model.LeaveTypes = await GetLeaveTypes(model.LeaveTypeId);
                return View(model);
            }

            var dto = model.Adapt<LeaveCreateDTO>();
            dto.AppUserId = await ResolveCurrentAppUserIdAsync(); // login’den set

            var result = await _leaveService.CreateAsync(dto);
            if (!result.IsSuccess)
            {
                await Console.Out.WriteLineAsync(result.Messages);
                model.LeaveTypes = await GetLeaveTypes(model.LeaveTypeId);
                return View(model);
            }

            await Console.Out.WriteLineAsync(result.Messages);
            return RedirectToAction(nameof(Index));
        }

        // DELETE
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _leaveService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                await Console.Out.WriteLineAsync(result.Messages);
                return RedirectToAction(nameof(Index));
            }

            await Console.Out.WriteLineAsync(result.Messages);
            return RedirectToAction(nameof(Index));
        }

        // UPDATE (GET)
        public async Task<IActionResult> Update(Guid id)
        {
            var result = await _leaveService.GetByIdAsync(id);
            if (!result.IsSuccess || result.Data == null)
            {
                await Console.Out.WriteLineAsync(result.Messages);
                return RedirectToAction(nameof(Index));
            }

            var vm = result.Data.Adapt<LeaveEditVM>();
            vm.LeaveTypes = await GetLeaveTypes(vm.LeaveTypeId);
            return View(vm);
        }

        // UPDATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(LeaveEditVM model)
        {
            if (!ModelState.IsValid)
            {
                model.LeaveTypes = await GetLeaveTypes(model.LeaveTypeId);
                return View(model);
            }

            var dto = model.Adapt<LeaveUpdateDTO>();
            var result = await _leaveService.UpdateAsync(dto);

            if (!result.IsSuccess)
            {
                await Console.Out.WriteLineAsync(result.Messages);
                model.LeaveTypes = await GetLeaveTypes(model.LeaveTypeId);
                return View(model);
            }

            await Console.Out.WriteLineAsync(result.Messages);
            return RedirectToAction(nameof(Index));
        }

        // APPROVE
        public async Task<IActionResult> ApproveLeave(Guid id)
        {
            var result = await _leaveService.ApproveLeaveAsync(id);
            if (!result.IsSuccess)
            {
                await Console.Out.WriteLineAsync(result.Messages);
                return RedirectToAction(nameof(Index));
            }
            await Console.Out.WriteLineAsync(result.Messages);
            return RedirectToAction(nameof(Index));
        }

        // REJECT
        public async Task<IActionResult> RejectLeave(Guid id)
        {
            var result = await _leaveService.RejectLeaveAsync(id);
            if (!result.IsSuccess)
            {
                await Console.Out.WriteLineAsync(result.Messages);
                return RedirectToAction(nameof(Index));
            }
            await Console.Out.WriteLineAsync(result.Messages);
            return RedirectToAction(nameof(Index));
        }

        // ----------------- helpers -----------------
        private async Task<SelectList> GetLeaveTypes(Guid? selectedId = null)
        {
            var res = await _leaveTypeService.GetAllAsync();
            var data = res.IsSuccess && res.Data != null ? res.Data : new List<LeaveTypeListDTO>();

            var items = data
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name, // <-- Type değil Name
                    Selected = selectedId.HasValue && x.Id == selectedId.Value
                })
                .OrderBy(x => x.Text)
                .ToList();

            return new SelectList(items, "Value", "Text", selectedId?.ToString());
        }

        private async Task<Guid> ResolveCurrentAppUserIdAsync()
        {
            var email = User.FindFirstValue(ClaimTypes.Email)
                     ?? User.FindFirst("preferred_username")?.Value
                     ?? User.FindFirst(ClaimTypes.Upn)?.Value;

            if (string.IsNullOrWhiteSpace(email))
                throw new InvalidOperationException("User email claim not found.");

            var user = await _appUserService.GetByEmailAsync(email);
            if (!user.IsSuccess || user.Data == null)
                throw new InvalidOperationException("AppUser not found for current user.");

            return user.Data.Id;
        }
    }
}
