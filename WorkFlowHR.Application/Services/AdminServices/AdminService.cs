using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowHR.Application.DTOs.AdminDTOs;
using WorkFlowHR.Application.DTOs.ChangePasswordDTOs;
using WorkFlowHR.Application.Services.AccountServices;
using WorkFlowHR.Domain.Utilities.Concretes;
using WorkFlowHR.Domain.Utilities.Interfaces;
using WorkFlowHR.Infrastructure.Repositories.AdminRepositories;

namespace WorkFlowHR.Application.Services.AdminServices
{
    public class AdminService : IAdminService
    {
        private readonly IAccountService _accountService;
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAccountService accountService, IAdminRepository adminRepository)
        {
            _accountService = accountService;
            _adminRepository = adminRepository;
        }
        public async Task<IResult> ChangePasswordAsync(AdminChangePasswordDTO adminChangePasswordDTO)
        {
            var admin = await _adminRepository.GetByIdAsync(adminChangePasswordDTO.Id);
            if (admin == null)
            {
                return new ErrorResult("Admin bulunamadı!");
            }

            var user = await _accountService.FindByIdAsync(admin.IdentityId);
            if (user == null)
            {
                return new ErrorResult("Kullanıcı bulunamadı!");
            }

            var result = await _accountService.ChangePasswordAsyncc(user, adminChangePasswordDTO.OldPassword, adminChangePasswordDTO.NewPassword);
            if (!result.Succeeded)
            {
                return new ErrorResult("Şifre değiştirme başarısız: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return new SuccessResult("Şifre başarıyla değiştirildi.");
        }

        public async Task<IDataResult<List<AdminListDTO>>> GetAllAsync()
        {
            var admins = await _adminRepository.GetAllAsync();
            var adminDTOs = admins.Adapt<List<AdminListDTO>>();
            if (admins.Count() <= 0)
            {
                return new ErrorDataResult<List<AdminListDTO>>(adminDTOs, "Görüntülenecek Kullanıcı Bulunamadı");
            }
            return new SuccessDataResult<List<AdminListDTO>>(adminDTOs, "Kullanıcılar Listelendi");
        }

        public async Task<IDataResult<AdminDTO>> GetByIdAsync(Guid id)
        {
            try
            {
                var admin = await _adminRepository.GetByIdAsync(id);
                if (admin == null)
                {
                    return new ErrorDataResult<AdminDTO>("Kullanıcı Bulunamadı");
                }

                var adminDTO = admin.Adapt<AdminDTO>();

                return new SuccessDataResult<AdminDTO>(adminDTO, "Kullanıcı Başarıyla Getirildi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<AdminDTO>("Kullanıcı Getirme İşlemi Başarısız: " + ex.Message);
            }
        }

        public async Task<IDataResult<AdminDTO>> GetByIdentityUserIdAsync(string identityUserId)
        {
            var admin = await _adminRepository.GetByIdentityId(identityUserId);
            if (admin == null)
            {
                return new ErrorDataResult<AdminDTO>("Admin bulunamadı!");
            }
            var adminDTO = admin.Adapt<AdminDTO>();
            return new SuccessDataResult<AdminDTO>(adminDTO, "Admin başarıyla bulundu!");
        }
    }
}
