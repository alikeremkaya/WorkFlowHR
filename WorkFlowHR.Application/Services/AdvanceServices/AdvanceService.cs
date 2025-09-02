using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using WorkFlowHR.Application.DTOs.AdvanceDTOs;
using WorkFlowHR.Application.DTOs.MailDTOs;
using WorkFlowHR.Application.Services.AppUserServices;
using WorkFlowHR.Application.Services.MailServices;
using WorkFlowHR.Domain.Entities;
using WorkFlowHR.Domain.Enums;
using WorkFlowHR.Domain.Utilities.Concretes;
using WorkFlowHR.Domain.Utilities.Interfaces;
using WorkFlowHR.Infrastructure.Repositories.AdvanceRepositories;

namespace WorkFlowHR.Application.Services.AdvanceServices
{
    public class AdvanceService:IAdvanceService
    {
        private readonly IAdvanceRepository _advanceRepository;
        private readonly ILogger<AdvanceService> _logger;
        private readonly IMailService _mailService;
        private readonly IAppUserService _appUserService;
       

        public AdvanceService(IAdvanceRepository advanceRepository, ILogger<AdvanceService> logger, IMailService mailService,  IAppUserService appUserService)
        {
            _advanceRepository = advanceRepository;
            _logger = logger;
            _mailService = mailService;
           
             
            _appUserService = appUserService;
        }

        public async Task<IDataResult<AdvanceDTO>> CreateAsync(AdvanceCreateDTO advanceCreateDTO)
        {
            var newAdvance = advanceCreateDTO.Adapt<Advance>();
            newAdvance.AdvanceStatus = AdvanceStatus.Pending;

            try
            {
                await _advanceRepository.AddAsync(newAdvance);
                await _advanceRepository.SaveChangesAsync();

                var manager = await _appUserService.GetByIdAsync(newAdvance.AppUserId);

                if (manager?.Data != null && !string.IsNullOrWhiteSpace(manager.Data.Email))
                {
                    var mailDTO = new MailDTO
                    {
                        Email = manager.Data.Email,
                        Subject = "New Advance Request Created",
                        Message = "A new advance request has been created and is pending your approval."
                    };

                    await _mailService.SendMailAsync(mailDTO);
                }

                return new SuccessDataResult<AdvanceDTO>(newAdvance.Adapt<AdvanceDTO>(), "Avans başarıyla eklendi.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Avans eklenirken bir hata oluştu.");
                return new ErrorDataResult<AdvanceDTO>(newAdvance.Adapt<AdvanceDTO>(), "Avans ekleme başarısız: " + ex.Message);
            }
        }



        public async Task<IResult> DeleteAsync(Guid id)
        {
            var deletingAdvance = await _advanceRepository.GetByIdAsync(id);

            if (deletingAdvance is null)
            {
                return new ErrorResult("Silinecek avans bulunamadı.");
            }

            await _advanceRepository.DeleteAsync(deletingAdvance);
            await _advanceRepository.SaveChangesAsync();

            if (deletingAdvance.AppUser != null && !string.IsNullOrEmpty(deletingAdvance.AppUser.Email) && deletingAdvance.AppUser.Role == Roles.Employee.ToString())
            {
                var mailDTO = new MailDTO
                {
                    Email = deletingAdvance.AppUser.Email,
                    Subject = "Advance Deleted",
                    Message = "An advance request for one of your employees has been deleted."
                };
                await _mailService.SendMailAsync(mailDTO);
            }

            return new SuccessResult("Avans başarıyla silindi.");
        }

        public async Task<IDataResult<List<AdvanceListDTO>>> GetAllAsync()
        {
            var advances = await _advanceRepository.GetAllAsync(x => x.CreatedDate, true);

            if (advances.Count() <= 0)
            {
                return new ErrorDataResult<List<AdvanceListDTO>>(advances.Adapt<List<AdvanceListDTO>>(), "Listelenecek avans bulunamadı.");
            }

            var advanceListDTOs = new List<AdvanceListDTO>();

            foreach (var advance in advances)
            {
                var advanceListDTO = advance.Adapt<AdvanceListDTO>();

               
               
               
                advanceListDTOs.Add(advanceListDTO);
            }

            return new SuccessDataResult<List<AdvanceListDTO>>(advanceListDTOs, "Avans listeleme başarılı.");
        }

        public async Task<IDataResult<AdvanceDTO>> GetByIdAsync(Guid id)
        {
            var advance = await _advanceRepository.GetByIdAsync(id);

            if (advance is null)
            {
                return new ErrorDataResult<AdvanceDTO>("Gösterilecek avans bulunamadı.");
            }
            var advanceDto = advance.Adapt<AdvanceDTO>();

            return new SuccessDataResult<AdvanceDTO>(advanceDto, "Avans gösterme işlemi başarılı.");
        }

        public async Task<IDataResult<AdvanceDTO>> UpdateAsync(AdvanceUpdateDTO advanceUpdateDTO)
        {
            var advance = await _advanceRepository.GetByIdAsync(advanceUpdateDTO.Id);
            if (advance == null)
            {
                return new ErrorDataResult<AdvanceDTO>("Güncellenecek avans bulunamadı.");
            }

            advance.Amount = advanceUpdateDTO.Amount;
            advance.AdvanceDate = advanceUpdateDTO.AdvanceDate;
            advance.AppUserId = advanceUpdateDTO.AppUserId;

            if (advanceUpdateDTO.Image != null && advanceUpdateDTO.Image.Length > 0)
            {
                advance.Image = advanceUpdateDTO.Image;
            }

            advance.AdvanceStatus = advanceUpdateDTO.AdvanceStatus;

            try
            {
                await _advanceRepository.UpdateAsync(advance);
                await _advanceRepository.SaveChangesAsync();
                return new SuccessDataResult<AdvanceDTO>(advance.Adapt<AdvanceDTO>(), "Avans güncelleme başarılı.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Avans güncellenirken bir hata oluştu.");
                return new ErrorDataResult<AdvanceDTO>("Avans güncelleme sırasında bir hata oluştu.");
            }
        }

        public async Task<IResult> ApproveAsync(Guid id)
        {
            var advance = await _advanceRepository.GetByIdAsync(id);
            if (advance == null)
            {
                return new ErrorResult("Onaylanacak avans bulunamadı.");
            }

            advance.AdvanceStatus = AdvanceStatus.Approved;

            try
            {
                await _advanceRepository.UpdateAsync(advance);
                await _advanceRepository.SaveChangesAsync();

                // E-posta gönderme işlemi
                if (advance.AppUser != null && !string.IsNullOrEmpty(advance.AppUser.Email))
                {
                    var mailDTO = new MailDTO
                    {
                        Email = advance.AppUser.Email,
                        Subject = "Advance Approved",
                        Message = "Your advance request has been approved."
                    };
                    await _mailService.SendMailAsync(mailDTO);
                }

                return new SuccessResult("Avans onaylama başarılı.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Avans onaylanırken bir hata oluştu.");
                return new ErrorResult("Avans onaylama sırasında bir hata oluştu.");
            }
        }
        public async Task<IDataResult<List<AdvanceListDTO>>> GetAllByManagerIdAsync(Guid managerId)
        {
            var advances = await _advanceRepository.GetAllAsync(x => x.AppUserId == managerId);
            if (advances == null || !advances.Any())
            {
                return new ErrorDataResult<List<AdvanceListDTO>>(advances.Adapt<List<AdvanceListDTO>>(), "Bu yönetici için avans bulunamadı.");
            }
            return new SuccessDataResult<List<AdvanceListDTO>>(advances.Adapt<List<AdvanceListDTO>>(), "Avans listeleme başarılı.");
        }

        public async Task<IResult> RejectAsync(Guid id)
        {
            var advance = await _advanceRepository.GetByIdAsync(id);
            if (advance == null)
            {
                return new ErrorResult("Reddedilecek avans bulunamadı.");
            }

            advance.AdvanceStatus = AdvanceStatus.Rejected;

            try
            {
                await _advanceRepository.UpdateAsync(advance);
                await _advanceRepository.SaveChangesAsync();

                // E-posta gönderme işlemi
                if (advance.AppUser != null && !string.IsNullOrEmpty(advance.AppUser.Email))
                {
                    var mailDTO = new MailDTO
                    {
                        Email = advance.AppUser.Email,
                        Subject = "Advance Rejected",
                        Message = "Your advance request has been rejected."
                    };
                    await _mailService.SendMailAsync(mailDTO);
                }

                return new SuccessResult("Avans reddetme başarılı.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Avans reddedilirken bir hata oluştu.");
                return new ErrorResult("Avans reddetme sırasında bir hata oluştu.");
            }
        }
    }
}
