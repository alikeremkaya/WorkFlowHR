using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WorkFlowHR.Application.DTOs.AppUserDTOs;
using WorkFlowHR.Domain.Entities;
using WorkFlowHR.Domain.Utilities.Interfaces;

namespace WorkFlowHR.Application.Services.AppUserServices
{
    public interface IAppUserService
    {
        /// <summary>
        /// Manager alanı için tüm kullanıcıları (liste biçiminde DTO olarak) geri döner.
        /// </summary>
        Task<IDataResult<List<AppUserListDTO>>> GetAllAsync();

        /// <summary>
        /// Yeni bir kullanıcı oluşturur ve oluşturulan DTO’yu döner.
        /// </summary>
        Task<IDataResult<AppUserDTO>> CreateAsync(AppUserCreateDTO createDto);

        /// <summary>
        /// ID’si verilen kullanıcıyı DTO olarak döner.
        /// </summary>
        Task<IDataResult<AppUserDTO>> GetByIdAsync(Guid id);

        /// <summary>
        /// Mevcut kullanıcıyı günceller.
        /// </summary>
        Task<IResult> UpdateAsync(AppUserUpdateDTO updateDto);

        /// <summary>
        /// Azure AD kimliğiyle gelen ClaimsPrincipal’dan AppUser kaydını oluşturur/günceller ve DTO olarak döner.
        /// </summary>
        Task<IDataResult<AppUserDTO>> EnsureAppUserAsync(ClaimsPrincipal user);

        /// <summary>
        /// Verilen email’e karşılık gelen rolü ("Manager"/"Employee") döner.
        /// </summary>
        Task<IDataResult<string>> GetRoleAsync(string email);

        /// <summary>
        /// Email’e göre kullanıcıyı DTO olarak döner.
        /// </summary>
        Task<IDataResult<AppUserDTO>> GetByEmailAsync(string email);
    }
}
