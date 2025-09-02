using Mapster;
using Microsoft.Extensions.Logging;
using WorkFlowHR.Application.DTOs.LeaveDTOs;
using WorkFlowHR.Application.DTOs.MailDTOs;
using WorkFlowHR.Application.Services.AppUserServices;
using WorkFlowHR.Application.Services.MailServices;
using WorkFlowHR.Domain.Entities;
using WorkFlowHR.Domain.Enums;
using WorkFlowHR.Domain.Utilities.Concretes;
using WorkFlowHR.Domain.Utilities.Interfaces;
using WorkFlowHR.Infrastructure.Repositories.LeaveRepositories;

namespace WorkFlowHR.Application.Services.LeaveServices
{

    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRepository _leaveRepository;
        private readonly ILogger<LeaveService> _logger;
        private readonly IMailService _mailService;
        private readonly IAppUserService _appUserService;

        public LeaveService(
            ILeaveRepository leaveRepository,
            ILogger<LeaveService> logger,
            IMailService mailService,
            IAppUserService appUserService)
        {
            _leaveRepository = leaveRepository;
            _logger = logger;
            _mailService = mailService;
            _appUserService = appUserService;
        }
        public async Task<IDataResult<List<LeaveListDTO>>> GetPendingLeavesAsync()
        {
            var leaves = await _leaveRepository.GetAllAsync(x => x.LeaveStatus == LeaveStatus.Pending);
            // Mapster'ın Adapt extension metodunu kullanıyoruz.
            var leavesDto = leaves.Adapt<List<LeaveListDTO>>();
            return new SuccessDataResult<List<LeaveListDTO>>(leavesDto, "Onay bekleyen izinler başarıyla getirildi.");
        }

        public async Task<IDataResult<List<LeaveListDTO>>> GetApprovedLeavesAsync()
        {
            var leaves = await _leaveRepository.GetAllAsync(x => x.LeaveStatus == LeaveStatus.Approved);
            var leavesDto = leaves.Adapt<List<LeaveListDTO>>();
            return new SuccessDataResult<List<LeaveListDTO>>(leavesDto, "Onaylanmış izinler başarıyla getirildi.");
        }

        public async Task<IDataResult<List<LeaveListDTO>>> GetRejectedLeavesAsync()
        {
            var leaves = await _leaveRepository.GetAllAsync(x => x.LeaveStatus == LeaveStatus.Rejected);
            var leavesDto = leaves.Adapt<List<LeaveListDTO>>();
            return new SuccessDataResult<List<LeaveListDTO>>(leavesDto, "Reddedilmiş izinler başarıyla getirildi.");
        }

        public async Task<IDataResult<List<LeaveListDTO>>> GetUpcomingLeavesAsync()
        {
            // Yaklaşan izinler: Onaylanmış ve başlangıç tarihi bugünden sonra olanlar.
            var leaves = await _leaveRepository.GetAllAsync(x => x.LeaveStatus == LeaveStatus.Approved && x.StartDate > DateTime.Today);
            var leavesDto = leaves.Adapt<List<LeaveListDTO>>();
            return new SuccessDataResult<List<LeaveListDTO>>(leavesDto, "Yaklaşan izinler başarıyla getirildi.");
        }

        public async Task<IDataResult<List<LeaveListDTO>>> GetAllAsync()
        {
            var leaves = await _leaveRepository.GetAllAsync(); 
            if (leaves == null)
                return new ErrorDataResult<List<LeaveListDTO>>("Listeleme başarısız");

            var list = leaves.Adapt<List<LeaveListDTO>>();
            return new SuccessDataResult<List<LeaveListDTO>>(list, "Listeleme başarılı.");
        }

        public async Task<IDataResult<LeaveDTO>> CreateAsync(LeaveCreateDTO dto)
        {
            var entity = dto.Adapt<Leave>();
            entity.LeaveStatus = LeaveStatus.Pending;

            try
            {
                await _leaveRepository.AddAsync(entity);
                await _leaveRepository.SaveChangesAsync();

                var creator = await _appUserService.GetByIdAsync(entity.AppUserId);
                if (creator.IsSuccess && creator.Data != null && !string.IsNullOrEmpty(creator.Data.Email))
                {
                    var mailDTO = new MailDTO
                    {
                        Email = creator.Data.Email,
                        Subject = "Leave Request Created",
                        Message = "Your leave request has been created and is pending approval."
                    };
                    await _mailService.SendMailAsync(mailDTO);
                }

                return new SuccessDataResult<LeaveDTO>(entity.Adapt<LeaveDTO>(), "İzin başarıyla eklendi.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "İzin eklenirken hata.");
                return new ErrorDataResult<LeaveDTO>("İzin ekleme başarısız: " + ex.Message);
            }
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = await _leaveRepository.GetByIdAsync(id);
            if (entity == null)
                return new ErrorResult("Silinecek izin bulunamadı.");

            await _leaveRepository.DeleteAsync(entity);
            await _leaveRepository.SaveChangesAsync();

            if (entity.AppUser != null && !string.IsNullOrEmpty(entity.AppUser.Email))
            {
                var mailDTO = new MailDTO
                {
                    Email = entity.AppUser.Email,
                    Subject = "Leave Deleted",
                    Message = "Your leave request has been deleted."
                };
                await _mailService.SendMailAsync(mailDTO);
            }

            return new SuccessResult("İzin başarıyla silindi.");
        }

        public async Task<IDataResult<LeaveDTO>> GetByIdAsync(Guid id)
        {
            var entity = await _leaveRepository.GetByIdAsync(id);
            if (entity == null)
                return new ErrorDataResult<LeaveDTO>("Veri bulunamadı.");

            var dto = entity.Adapt<LeaveDTO>();
            return new SuccessDataResult<LeaveDTO>(dto, "Veri başarıyla bulundu");
        }

        public async Task<IDataResult<LeaveDTO>> UpdateAsync(LeaveUpdateDTO dto)
        {
            var entity = await _leaveRepository.GetByIdAsync(dto.Id);
            if (entity == null)
                return new ErrorDataResult<LeaveDTO>("Güncellenecek izin bulunamadı.");

            entity.StartDate = dto.StartDate;
            entity.EndDate = dto.EndDate;
            entity.LeaveTypeId = dto.LeaveTypeId;
            entity.AppUserId = dto.AppUserId;
            entity.LeaveStatus = dto.LeaveStatus;

            try
            {
                await _leaveRepository.UpdateAsync(entity);
                await _leaveRepository.SaveChangesAsync();
                return new SuccessDataResult<LeaveDTO>(entity.Adapt<LeaveDTO>(), "İzin güncelleme başarılı.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "İzin güncellenirken hata.");
                return new ErrorDataResult<LeaveDTO>("İzin güncelleme sırasında bir hata oluştu.");
            }
        }

        public async Task<IResult> ApproveLeaveAsync(Guid id)
        {
            var entity = await _leaveRepository.GetByIdAsync(id);
            if (entity == null)
                return new ErrorResult("Onaylanacak izin bulunamadı.");

            entity.LeaveStatus = LeaveStatus.Approved;

            try
            {
                await _leaveRepository.UpdateAsync(entity);
                await _leaveRepository.SaveChangesAsync();

                if (entity.AppUser != null && !string.IsNullOrEmpty(entity.AppUser.Email))
                {
                    var mailDTO = new MailDTO
                    {
                        Email = entity.AppUser.Email,
                        Subject = "Leave Approved",
                        Message = "Your leave request has been approved."
                    };
                    await _mailService.SendMailAsync(mailDTO);
                }

                return new SuccessResult("İzin Onaylama Başarılı");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "İzin onaylanırken hata.");
                return new ErrorResult("İzin onaylama sırasında bir hata oluştu.");
            }
        }

        public async Task<IResult> RejectLeaveAsync(Guid id)
        {
            var entity = await _leaveRepository.GetByIdAsync(id);
            if (entity == null)
                return new ErrorResult("İzin bulunamadı.");

            entity.LeaveStatus = LeaveStatus.Rejected;

            try
            {
                await _leaveRepository.UpdateAsync(entity);
                await _leaveRepository.SaveChangesAsync();

                if (entity.AppUser != null && !string.IsNullOrEmpty(entity.AppUser.Email))
                {
                    var mailDTO = new MailDTO
                    {
                        Email = entity.AppUser.Email,
                        Subject = "Leave Rejected",
                        Message = "Your leave request has been rejected."
                    };
                    await _mailService.SendMailAsync(mailDTO);
                }

                return new SuccessResult("İzin reddetme başarılı.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "İzin reddedilirken hata.");
                return new ErrorResult("İzin reddetme sırasında bir hata oluştu.");
            }
        }
    }
}


