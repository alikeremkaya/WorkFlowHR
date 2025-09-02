using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorkFlowHR.Application.DTOs.AppUserDTOs;
using WorkFlowHR.Application.Services.AppUserServices;
using WorkFlowHR.Domain.Enums;
using WorkFlowHR.UI.Areas.Manager.Models.EmployeeVMs;


namespace WorkFlowHR.UI.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Policy = "ManagerOnly")]
    public class EmployeeController : Controller
    {
        private readonly IAppUserService _userService;

        public EmployeeController(IAppUserService userService)
        {

            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);



            var result = await _userService.GetAllAsync();

            if (!result.IsSuccess)
            {
                await Console.Out.WriteLineAsync(result.Messages);
                return View(new List<EmployeeListVM>());
            }

            var employeeVMs = result.Data
                .Where(m =>
                    string.Equals(m.Role, Roles.Employee.ToString(), StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(m.Role, Roles.Manager.ToString(), StringComparison.OrdinalIgnoreCase))
                .Adapt<List<EmployeeListVM>>();

            await Console.Out.WriteLineAsync(result.Messages);
            return View(employeeVMs);
        }
        [HttpGet]
        public async Task<IActionResult> UpdatePhoto(Guid id)
        {
            var userRes = await _userService.GetByIdAsync(id);
            if (!userRes.IsSuccess || userRes.Data is null)
            {
                // AJAX çağrısına hata olarak NotFound() dönmek daha doğrudur.
                return NotFound("User not found.");
            }

            var vm = new EmployeeUpdatePhotoVM
            {
                Id = userRes.Data.Id,
                DisplayName = userRes.Data.DisplayName,
                Email = userRes.Data.Email,
                ExistingImage = userRes.Data.Image
            };

            // Doğru kullanım: Sadece formu içeren partial view'ı döndürür.
            return PartialView("UpdatePhoto", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePhoto(EmployeeUpdatePhotoVM vm)
        {
            // Model validasyon hataları için
            if (!ModelState.IsValid)
            {
                // Hataları göstermek için formu PartialView olarak geri döndür
                return PartialView(vm);
            }

            if (vm.NewImage is null || vm.NewImage.Length == 0)
            {
                ModelState.AddModelError(nameof(vm.NewImage), "Please choose an image.");
                return PartialView(vm);
            }

            if (!vm.NewImage.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(nameof(vm.NewImage), "Invalid image type.");
                return PartialView(vm);
            }

            using var ms = new MemoryStream();
            await vm.NewImage.CopyToAsync(ms);
            var dto = new AppUserUpdatePhotoDTO { Id = vm.Id, Image = ms.ToArray() };

            var res = await _userService.UpdatePhotoAsync(dto);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, res.Messages);
                return PartialView(vm);
            }

            TempData["Success"] = "Photo updated.";
            return Json(new { success = true });
        }
    }
    }
