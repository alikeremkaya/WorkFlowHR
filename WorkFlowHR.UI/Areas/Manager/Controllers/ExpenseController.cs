using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WorkFlowHR.Application.DTOs.ExpenseDTOs;
using WorkFlowHR.Application.Services.AppUserServices;
using WorkFlowHR.Application.Services.ExpenseServices;
using WorkFlowHR.Domain.Utilities.Concretes;
using WorkFlowHR.UI.Areas.Employee.Models.AdvanceVMs;
using WorkFlowHR.UI.Areas.Manager.Models.ExpenseVMs;
using WorkFlowHR.UI.Extentions;

namespace WorkFlowHR.UI.Areas.Manager.Controllers
    {
        [Area("Manager")]
        [Authorize(Policy = "ManagerOnly")]
        public class ExpenseController : Controller
        {
        private readonly IExpenseService _expenseService;
        private readonly IAppUserService _userService;

        public ExpenseController(IExpenseService expenseService, IAppUserService appUserService)
        {
            _expenseService = expenseService;
            _userService = appUserService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var res = await _expenseService.GetAllAsync();
            var vms = (res.Data ?? new List<ExpenseListDTO>()).Adapt<List<ExpenseListVM>>();
            return View(vms);
        }
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _expenseService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                await Console.Out.WriteLineAsync(result.Messages);
                return RedirectToAction("Index");
            }

            var expenseDetailsVM = result.Data.Adapt<ExpenseDetailsVM>();
            return View(expenseDetailsVM);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new ExpenseCreateVM
            {
                Managers = await GetManagers()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExpenseCreateVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var dto = new ExpenseCreateDTO
            {
                Amount = vm.Amount,
                ExpenseDate = vm.ExpenseDate,
                Description = vm.Description,
                AppUserId = await ResolveCurrentAppUserIdAsync()
            };

            var file = vm.NewImage ?? vm.NewImage;
            if (file != null && file.Length > 0)
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                dto.Image = ms.ToArray();   
            }

           

            var res = await _expenseService.CreateAsync(dto);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, res.Messages);
                return View(vm);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var res = await _expenseService.GetByIdAsync(id);


            if (!res.IsSuccess || res.Data == null)
                return NotFound();

          
            var vm = res.Data.Adapt<ExpenseUpdateVM>();
            vm.ManagerId = res.Data.AppUserId;


            vm.Managers = await GetManagers(vm.ManagerId);
            vm.ExistingImage = res.Data.Image;
          

            return PartialView(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ExpenseUpdateVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var dto = vm.Adapt<ExpenseUpdateDTO>();

            if (vm.NewImage != null && vm.NewImage.Length > 0)
            {
                using var ms = new MemoryStream();
                await vm.NewImage.CopyToAsync(ms);
                dto.Image = ms.ToArray();
            }

            if (dto.AppUserId == Guid.Empty)
                dto.AppUserId = await ResolveCurrentAppUserIdAsync();

            var res = await _expenseService.UpdateAsync(dto);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, res.Messages);
                return View(vm);
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success = true, redirectUrl = Url.Action(nameof(Index)) });

            TempData["Success"] = "Expense updated.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _expenseService.DeleteAsync(id);

                if (result.IsSuccess)
                {
                    return Json(new { success = true, message = "The expense request has been deleted." });
                }
                else
                {
                    return Json(new { success = false, message = result.Messages ?? "An error occurred while deleting the expense request." });
                }
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "An unexpected server error occurred: " + ex.Message });
            }
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




