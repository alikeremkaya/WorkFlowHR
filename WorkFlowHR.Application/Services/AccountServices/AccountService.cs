using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WorkFlowHR.Domain.Core.Base;
using WorkFlowHR.Domain.Enums;
using WorkFlowHR.Infrastructure.Repositories.AdminRepositories;
using WorkFlowHR.Infrastructure.Repositories.ManagerRepositories;

namespace WorkFlowHR.Application.Services.AccountServices
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAdminRepository _adminRepository;
        private readonly IManagerRepository _managerRepository;

        public AccountService(UserManager<IdentityUser> userManager, IAdminRepository adminRepository, IManagerRepository managerRepository)
        {
            _userManager = userManager;
            _adminRepository = adminRepository;
            _managerRepository = managerRepository;
        }

        public async Task<bool> AnyAsync(Expression<Func<IdentityUser, bool>> expression)
        {
            return await _userManager.Users.AnyAsync(expression);
        }

        public async Task<IdentityResult> ChangePasswordAsyncc(IdentityUser user, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task<IdentityResult> CreateUserAsync(IdentityUser user, Roles role)
        {
            var result = await _userManager.CreateAsync(user, "Password.1");
            if (!result.Succeeded)
            {
                return result;

            }
            return await _userManager.AddToRoleAsync(user, role.ToString());
        }

        public async Task<IdentityResult> DeleteUserAsync(string identityId)
        {
            var user = await _userManager.FindByIdAsync(identityId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError()
                {
                    Code = "Kullanıcı Bulunamadı",
                    Description = "Kullanıcı Bulunamadı",

                });
            }
            return await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityUser?> FindByIdAsync(string identityId)
        {
            return await _userManager.FindByIdAsync(identityId);
        }

        public async Task<Guid> GetUserIdAsync(string identityId, string role)
        {
            BaseUser? user = role switch
            {
                "Admin" => await _adminRepository.GetByIdentityId(identityId),
                "Manager" => await _managerRepository.GetByIdentityId(identityId),
                _ => null,


            };
            return user is null ? Guid.NewGuid() : user.Id;
        }

        public async Task<IdentityResult> UpdateUserAsync(IdentityUser user)
        {
            return await _userManager.UpdateAsync(user);
        }
    }
}
