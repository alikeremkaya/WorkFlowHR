using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WorkFlowHR.Application.DTOs.AdvanceDTOs;
using WorkFlowHR.Application.Services.AdvanceServices;
using WorkFlowHR.Application.Services.AppUserServices;
using WorkFlowHR.UI.Areas.Manager.Models.AdvanceVMs;


namespace WorkFlowHR.UI.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Policy = "ManagerOnly")]
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

            if (vm.NewImage != null && vm.NewImage.Length > 0)
            {
                using var ms = new MemoryStream();
                await vm.NewImage.CopyToAsync(ms);
                dto.Image = ms.ToArray();
            }

            dto.AppUserId = await ResolveCurrentAppUserIdAsync();

            var res = await _advanceService.CreateAsync(dto);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, res.Messages);
                vm.Managers = await GetManagers(vm.ManagerAppUserId);
                return View(vm);
            }

            TempData["Success"] = "Advance created.";
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _advanceService.DeleteAsync(id);

                if (result.IsSuccess)
                {
                    return Json(new { success = true, message = "Avans talebi başarıyla silindi." });
                }
                else
                {
                    return Json(new { success = false, message = result.Messages ?? "Silme işlemi sırasında bir hata oluştu." });
                }
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "Sunucu tarafında beklenmeyen bir hata oluştu: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var res = await _advanceService.GetByIdAsync(id);
            if (!res.IsSuccess || res.Data == null)
            {
                TempData["Error"] = res.Messages;
                return RedirectToAction("Index");
            }

            var vm = res.Data.Adapt<AdvanceEditVM>();
            vm.ExistingImage = res.Data.Image;
            vm.Managers = await GetManagers(vm.ManagerId);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(AdvanceEditVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var dto = vm.Adapt<AdvanceUpdateDTO>();

            // Yeni görsel varsa byte[]'e çevir
            if (vm.NewImage != null && vm.NewImage.Length > 0)
            {
                using var ms = new MemoryStream();
                await vm.NewImage.CopyToAsync(ms);
                dto.Image = ms.ToArray();
            }

            if (dto.AppUserId == Guid.Empty)
                dto.AppUserId = await ResolveCurrentAppUserIdAsync();

            var res = await _advanceService.UpdateAsync(dto);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, res.Messages);
                return View(vm);
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success = true, redirectUrl = Url.Action(nameof(Index)) });

            TempData["Success"] = "Advance updated.";
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveAdvance(Guid id)
        {
            try
            {
                var result = await _advanceService.ApproveAsync(id);

                if (!result.IsSuccess)
                {
                    return Json(new { success = false, message = result.Messages });
                }

                return Json(new { success = true, message = "The advance request has been approved." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An unexpected error occurred." });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectAdvance(Guid id)
        {
            try
            {
                var result = await _advanceService.RejectAsync(id);

                if (!result.IsSuccess)
                {
                    return Json(new { success = false, message = result.Messages });
                }

                return Json(new { success = true, message = "The advance request has been rejected." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An unexpected error occurred." });
            }
        }
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _advanceService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                await Console.Out.WriteLineAsync(result.Messages);
                return RedirectToAction("Index");
            }

            var advanceDetailsVM = result.Data.Adapt<AdvanceDetailsVM>();
            return View(advanceDetailsVM);
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
