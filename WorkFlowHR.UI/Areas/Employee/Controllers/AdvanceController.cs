using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WorkFlowHR.Application.DTOs.AdvanceDTOs;
using WorkFlowHR.Application.Services.AdvanceServices;
using WorkFlowHR.Application.Services.AppUserServices;
using WorkFlowHR.UI.Areas.Employee.Models.AdvanceVMs;

namespace WorkFlowHR.UI.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Policy = "EmployeeOnly")]
    public class AdvanceController : Controller
    {
        private readonly IAdvanceService _advanceService;
        private readonly IAppUserService _userService;
        private readonly ILogger<AdvanceController> _logger;

        public AdvanceController(
            IAdvanceService advanceService,
            IAppUserService userService,
            ILogger<AdvanceController> logger)
        {
            _advanceService = advanceService;
            _userService = userService;
            _logger = logger;
        }


        public async Task<IActionResult> Index()
        {
            var res = await _advanceService.GetAllAsync();
            if (!res.IsSuccess || res.Data == null)
            {
                TempData["Error"] = res.Messages;
                return View(new List<AdvanceListVM>());
            }
            var vm = res.Data.Adapt<List<AdvanceListVM>>();
            return View(vm);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new AdvanceCreateVM
            {
                Managers = await GetManagers()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdvanceCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Managers = await GetManagers(vm.ManagerAppUserId);
                return View(vm);
            }

            var dto = vm.Adapt<AdvanceCreateDTO>();

            // ImageFile -> byte[]
            if (vm.NewImage != null)
            {
                using var ms = new MemoryStream();
                await vm.NewImage.CopyToAsync(ms);
                dto.Image = ms.ToArray();
            }

            dto.AppUserId = await ResolveCurrentAppUserIdAsync();

            var res = await _advanceService.CreateAsync(dto);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.Messages);
                vm.Managers = await GetManagers(vm.ManagerAppUserId);
                return View(vm);
            }

            TempData["Success"] = "Advance created.";
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var res = await _advanceService.GetByIdAsync(id);
            if (!res.IsSuccess || res.Data == null)
            {
                TempData["Error"] = res.Messages;
                return RedirectToAction(nameof(Index));
            }

            var vm = res.Data.Adapt<AdvanceUpdateVM>();
            vm.ExistingImage = res.Data.Image;
            vm.Managers = await GetManagers(vm.ManagerId);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(AdvanceUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Managers = await GetManagers(vm.ManagerId);
                return View(vm);
            }

            var dto = vm.Adapt<AdvanceUpdateDTO>();

            if (vm.NewImage != null)
            {
                using var ms = new MemoryStream();
                await vm.NewImage.CopyToAsync(ms);
                dto.Image = ms.ToArray();
            }
            else
            {
                dto.Image = vm.ExistingImage; 
            }

            var res = await _advanceService.UpdateAsync(dto);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.Messages);
                vm.Managers = await GetManagers(vm.ManagerId);
                return View(vm);
            }

            TempData["Success"] = "Advance updated.";
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _advanceService.DeleteAsync(id);

                if (!result.IsSuccess)
                {

                    return Json(new { success = false, message = result.Messages });
                }

                return Json(new { success = true, message = result.Messages });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "An unexpected error occurred." });
            }
        }

        public async Task<IActionResult> ApproveAdvance(Guid id)
        {
            var result = await _advanceService.ApproveAsync(id);

            if (!result.IsSuccess)
            {
                await Console.Out.WriteLineAsync(result.Messages);
                return RedirectToAction("Index");
            }

            await Console.Out.WriteLineAsync(result.Messages);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> RejectAdvance(Guid id)
        {
            var result = await _advanceService.RejectAsync(id);

            if (!result.IsSuccess)
            {
                await Console.Out.WriteLineAsync(result.Messages);
                return RedirectToAction("Index");
            }

            await Console.Out.WriteLineAsync(result.Messages);
            return RedirectToAction("Index");
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
    }
}
