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
using WorkFlowHR.UI.Areas.Employee.Models.LeaveVMs;

namespace WorkFlowHR.UI.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Policy = "EmployeeOnly")]
    public class LeaveController : Controller
    {

        private readonly ILeaveTypeService _leaveTypeService;
        private readonly ILeaveService _leaveService;
        private readonly IAppUserService _userService;

        private readonly ILogger<LeaveController> _logger;

        public LeaveController(
            ILeaveTypeService leaveTypeService,
            ILeaveService leaveService,
            IAppUserService appUserService,
            ILogger<LeaveController> logger)
        {
            _leaveTypeService = leaveTypeService;
            _leaveService = leaveService;
            _userService = appUserService;
            _logger = logger;
        }

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

       

        public async Task<IActionResult> Create()
        {
            var vm = new LeaveCreateVM
            {
                LeaveTypes = await GetLeaveTypes(),
                Managers = await GetManagers(),

            };
            return View(vm);
        }

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
            dto.AppUserId = await ResolveCurrentAppUserIdAsync(); 

            var result = await _leaveService.CreateAsync(dto);
            if (!result.IsSuccess)
            {
                await Console.Out.WriteLineAsync(result.Messages);
                model.LeaveTypes = await GetLeaveTypes(model.LeaveTypeId);
                return View(model);
            }

            await Console.Out.WriteLineAsync(result.Messages);
            return RedirectToAction("Index");
        }

        // DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _leaveService.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = result.Messages });
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var result = await _leaveService.GetByIdAsync(id);
            if (!result.IsSuccess || result.Data == null)
            {
                await Console.Out.WriteLineAsync(result.Messages);
                return RedirectToAction("Index");
            }

            var vm = result.Data.Adapt<LeaveUpdateVM>();
            vm.LeaveTypes = await GetLeaveTypes(vm.LeaveTypeId);
            vm.Managers = await GetManagers(vm.ManagerId);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(LeaveUpdateVM model)
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
            return RedirectToAction("Index");
        }

        // APPROVE
        public async Task<IActionResult> ApproveLeave(Guid id)
        {
            var result = await _leaveService.ApproveLeaveAsync(id);
            if (!result.IsSuccess)
            {
                await Console.Out.WriteLineAsync(result.Messages);
                return RedirectToAction("Index");
            }
            await Console.Out.WriteLineAsync(result.Messages);
            return RedirectToAction("Index");
        }

        // REJECT
        public async Task<IActionResult> RejectLeave(Guid id)
        {
            var result = await _leaveService.RejectLeaveAsync(id);
            if (!result.IsSuccess)
            {
                await Console.Out.WriteLineAsync(result.Messages);
                return RedirectToAction("Index");
            }
            await Console.Out.WriteLineAsync(result.Messages);
            return RedirectToAction("Index");
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
        private async Task<SelectList> GetManagers(Guid? selectedId = null)
        {
            // Tüm AppUser’ları getir (rol filtrelemek istersen burada yaparsın)
            var res = await _userService.GetAllAsync();
            if (!res.IsSuccess || res.Data == null)
                return new SelectList(Enumerable.Empty<SelectListItem>());

            var items = res.Data
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = string.IsNullOrWhiteSpace(u.DisplayName) ? u.Email : u.DisplayName,
                    Selected = selectedId.HasValue && u.Id == selectedId.Value
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

            var user = await _userService.GetByEmailAsync(email);
            if (!user.IsSuccess || user.Data == null)
                throw new InvalidOperationException("AppUser not found for current user.");

            return user.Data.Id;
        }
    }
}
