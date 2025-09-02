using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkFlowHR.Application.DTOs.AppUserDTOs;
using WorkFlowHR.Application.Services.AppUserServices;
using WorkFlowHR.UI.Areas.Manager.Models.AppUserVMs;


namespace WorkFlowHR.UI.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Policy = "ManagerOnly")]
    public class AppUserController : Controller
    {
        private readonly IAppUserService _userService;
        private readonly IMapper _mapper;

        public AppUserController(IAppUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        
        public async Task<IActionResult> Index()
        {
            var result = await _userService.GetAllAsync();
            if (!result.IsSuccess)
                return View("Error", result.Messages);

            var vmList = result.Data.Adapt<List<AppUserListVM>>();
            return View(vmList);
        }

        
        [HttpGet]
        public IActionResult Create()
        {
            return View(new AppUserCreateVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppUserCreateVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var createDto = vm.Adapt<AppUserCreateDTO>();
            var res = await _userService.CreateAsync(createDto);
            if (!res.IsSuccess)
                return View("Error", res.Messages);

            return RedirectToAction(nameof(Index));
        }

      

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var res = await _userService.GetByIdAsync(id);
            if (!res.IsSuccess)
                return View("Error", res.Messages);

            var vm = res.Data.Adapt<AppUserEditVM>();
           
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AppUserEditVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var updateDto = vm.Adapt<AppUserUpdateDTO>();
            var res = await _userService.UpdateAsync(updateDto);
            if (!res.IsSuccess)
                return View("Error", res.Messages);

            return RedirectToAction(nameof(Index));
        }
    }
}
