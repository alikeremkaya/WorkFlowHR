using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WorkFlowHR.Application.DTOs.ExpenseDTOs;
using WorkFlowHR.Application.Services.AppUserServices;
using WorkFlowHR.Application.Services.ExpenseServices;
using WorkFlowHR.UI.Areas.Manager.Models.ExpenseVMs;
using WorkFlowHR.UI.Extentions;

namespace WorkFlowHR.UI.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Policy = "EmployeeOnly")]
    public class ExpenseController : Controller
    {
        private readonly IExpenseService _expenseService;
        private readonly IAppUserService _appUserService;
        private readonly ILogger<ExpenseController> _logger;

        public ExpenseController(
            IExpenseService expenseService,
            IAppUserService appUserService,
            ILogger<ExpenseController> logger)
        {
            _expenseService = expenseService;
            _appUserService = appUserService;
            _logger = logger;
        }

        // LIST
        public async Task<IActionResult> Index()
        {
            var result = await _expenseService.GetAllAsync();
            if (!result.IsSuccess || result.Data == null)
            {
                await Console.Out.WriteLineAsync(result.Messages);
                return View(new List<ExpenseListVM>());
            }

            var vms = result.Data.Adapt<List<ExpenseListVM>>();
            await Console.Out.WriteLineAsync(result.Messages);
            return View(vms);
        }

        // DETAILS


        // CREATE (GET)
        public async Task<IActionResult> Create()
        {
            var vm = new ExpenseCreateVM
            {
                Managers = await GetManagers()
            };
            return View(vm);
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExpenseCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Managers = await GetManagers(model.ManagerId);
                return View(model);
            }

            try
            {
                var dto = model.Adapt<ExpenseCreateDTO>();

                // Image
                if (model.NewImage != null && model.NewImage.Length > 0)
                {
                    dto.Image = await model.NewImage.StringToByteArrayAsync();
                }

                // Güvenlik: AppUserId login’den


                // VM’de ManagerId varsa, DTO’da ManagerAppUserId’ye eşle (alan adları farklı)
                dto.ManagerAppUserId = model.ManagerId;

                var result = await _expenseService.CreateAsync(dto);
                if (!result.IsSuccess)
                {
                    model.Managers = await GetManagers(model.ManagerId);
                    await Console.Out.WriteLineAsync(result.Messages);
                    return View(model);
                }

                await Console.Out.WriteLineAsync(result.Messages);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Expense create error");
                await Console.Out.WriteLineAsync($"Unexpected error: {ex.Message}");
                model.Managers = await GetManagers(model.ManagerId);
                return RedirectToAction("Index");
            }
        }

        // DELETE
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _expenseService.DeleteAsync(id);
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
            var result = await _expenseService.GetByIdAsync(id);
            if (!result.IsSuccess || result.Data == null)
            {
                await Console.Out.WriteLineAsync(result.Messages);
                return RedirectToAction(nameof(Index));
            }

            var vm = result.Data.Adapt<ExpenseEditVM>();
            vm.Managers = await GetManagers(vm.ManagerId);
            vm.ExistingImage = result.Data.Image;
            vm.Description = result.Data.Description;

            return View(vm);
        }

        // UPDATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ExpenseEditVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Managers = await GetManagers(model.ManagerId);
                return View(model);
            }

            var current = await _expenseService.GetByIdAsync(model.Id);
            if (!current.IsSuccess || current.Data == null)
            {
                await Console.Out.WriteLineAsync(current.Messages);
                return RedirectToAction(nameof(Index));
            }

            var dto = model.Adapt<ExpenseUpdateDTO>();

            // Yeni resim geldiyse değiştir, yoksa eskisini koru
            if (model.NewImage != null && model.NewImage.Length > 0)
                dto.Image = await model.NewImage.StringToByteArrayAsync();
            else
                dto.Image = current.Data.Image;

            // VM’de ManagerId → DTO’da ManagerAppUserId
            dto.ManagerAppUserId = model.ManagerId;

            var result = await _expenseService.UpdateAsync(dto);
            if (!result.IsSuccess)
            {
                await Console.Out.WriteLineAsync(result.Messages);
                model.Managers = await GetManagers(model.ManagerId);
                return View(model);
            }

            await Console.Out.WriteLineAsync(result.Messages);
            return RedirectToAction(nameof(Index));
        }

        // ----------------- helpers -----------------
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

        private async Task<SelectList> GetManagers(Guid? selectedId = null)
        {
            var res = await _appUserService.GetAllAsync();
            if (!res.IsSuccess || res.Data == null)
                return new SelectList(Enumerable.Empty<SelectListItem>());

            var items = res.Data
                // İstersen sadece Manager rolünü göster:
                // .Where(u => string.Equals(u.Role, "Manager", StringComparison.OrdinalIgnoreCase))
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
