using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowHR.Application.DTOs.AdminDTOs;
using WorkFlowHR.Application.DTOs.ChangePasswordDTOs;
using WorkFlowHR.Domain.Utilities.Interfaces;

namespace WorkFlowHR.Application.Services.AdminServices
{
    public interface IAdminService
    {
        /// <summary>
        /// Tüm adminleri getirir.
        /// </summary>
        /// <returns>Asenkron işlemi temsil eden bir görev. Görev sonucu, tüm adminlerin listesini içeren IDataResult nesnesini içerir.</returns>
        Task<IDataResult<List<AdminListDTO>>> GetAllAsync();


        /// <summary>
        /// Admini benzersiz kimliğine göre getirir.
        /// </summary>
        /// <param name="id">Adminin benzersiz kimliği.</param>
        /// <returns>Asenkron işlemi temsil eden bir görev. Görev sonucu, belirtilen kimliğe sahip adminin bilgilerini içeren IDataResult nesnesini içerir.</returns>
        Task<IDataResult<AdminDTO>> GetByIdAsync(Guid id);


        /// <summary>
        /// Belirtilen IdentityUser ID'sine sahip olan Admin verisini getirir.
        /// </summary>
        /// <param name="identityUserId">Kullanıcının IdentityUser ID'si</param>
        /// <returns>Oturum açan kullanıcının bilgilerini  getiren metod</returns>
        Task<IDataResult<AdminDTO>> GetByIdentityUserIdAsync(string identityUserId);

        Task<IResult> ChangePasswordAsync(AdminChangePasswordDTO adminChangePasswordDTO);
    }
}
