using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkFlowHR.Application.DTOs.LeaveTypeDTOs;
using WorkFlowHR.Application.Services.LeaveTypeServices;
using WorkFlowHR.UI.Areas.Manager.Models.LeaveTypeVMs;


namespace WorkFlowHR.UI.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Policy = "ManagerOnly")]
    public class LeaveTypeController : Controller
    {
        private readonly ILeaveTypeService _leaveTypeService;

        public LeaveTypeController(ILeaveTypeService leaveTypeService)
        {
            _leaveTypeService = leaveTypeService;
        }

      
        public async Task<IActionResult> Index()
        {
            var result = await _leaveTypeService.GetAllAsync();
            if (!result.IsSuccess)
            {
                await Console.Out.WriteLineAsync(result.Messages);
                return View(new List<LeaveTypeListVM>());
            }
            var leaveTypeVM = result.Data.Adapt<List<LeaveTypeListVM>>();
            await Console.Out.WriteLineAsync(result.Messages);
            return View(leaveTypeVM);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create(LeaveTypeCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            try
            {
                var leaveTypeDTO = model.Adapt<LeaveTypeCreateDTO>();
                var result = await _leaveTypeService.CreateAsync(leaveTypeDTO);
                if (!result.IsSuccess)
                {
                    await Console.Out.WriteLineAsync(result.Messages);
                    return RedirectToAction("Index");
                }
                await Console.Out.WriteLineAsync(result.Messages);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"Unexpected error: {ex.Message}");
                return RedirectToAction("Index");
            }
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _leaveTypeService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                await Console.Out.WriteLineAsync(result.Messages);
                return RedirectToAction("Index");
            }

            await Console.Out.WriteLineAsync(result.Messages);
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Update(Guid id)
        {
            var result = await _leaveTypeService.GetByIdAsync(id);
            var leaveTypeUpdateVM = result.Data.Adapt<LeaveTypeEditVM>();
            return View(leaveTypeUpdateVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(LeaveTypeEditVM model)

        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var result = await _leaveTypeService.UpdateAsync(model.Adapt<LeaveTypeEditDTO>());
            if (!result.IsSuccess)
            {
                await Console.Out.WriteLineAsync(result.Messages);
                return RedirectToAction("Index");
            }
            await Console.Out.WriteLineAsync(result.Messages);
            return RedirectToAction("Index");

        }
    }
}
