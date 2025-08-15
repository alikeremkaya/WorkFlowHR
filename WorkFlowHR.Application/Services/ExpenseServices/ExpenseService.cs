using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using WorkFlowHR.Application.DTOs.ExpenseDTOs;
using WorkFlowHR.Application.DTOs.MailDTOs;
using WorkFlowHR.Application.Services.AppUserServices;
using WorkFlowHR.Application.Services.MailServices;
using WorkFlowHR.Domain.Entities;
using WorkFlowHR.Domain.Enums;
using WorkFlowHR.Domain.Utilities.Concretes;
using WorkFlowHR.Domain.Utilities.Interfaces;
using WorkFlowHR.Infrastructure.Repositories.ExpenseRepositories;

namespace WorkFlowHR.Application.Services.ExpenseServices
{
    public class ExpenseService : IExpenseService
    {

        private readonly IExpenseRepository _expenseRepository;
        private readonly ILogger<ExpenseService> _logger;
        private readonly IMailService _mailService;
        private readonly IAppUserService _userService;

        public ExpenseService(
            IExpenseRepository expenseRepository,
            ILogger<ExpenseService> logger,
            IMailService mailService,
            IAppUserService userService)
        {
            _expenseRepository = expenseRepository;
            _logger = logger;
            _mailService = mailService;
            _userService = userService;
        }

        public async Task<IDataResult<ExpenseDTO>> CreateAsync(ExpenseCreateDTO dto)
        {
            var entity = dto.Adapt<Expense>(); 

            try
            {
                await _expenseRepository.AddAsync(entity);
                await _expenseRepository.SaveChangesAsync();

                // Sadece isteği oluşturan kullanıcıya bilgi maili
                var user = await _userService.GetByIdAsync(entity.AppUserId);
                if (user?.Data != null && !string.IsNullOrWhiteSpace(user.Data.Email))
                {
                    await _mailService.SendMailAsync(new MailDTO
                    {
                        Email = user.Data.Email,
                        Subject = "Expense Created",
                        Message = "Your expense request has been created."
                    });
                }

                return new SuccessDataResult<ExpenseDTO>(entity.Adapt<ExpenseDTO>(), "Harcama başarıyla eklendi.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Harcama eklenirken bir hata oluştu.");
                return new ErrorDataResult<ExpenseDTO>(entity.Adapt<ExpenseDTO>(), "Harcama ekleme başarısız: " + ex.Message);
            }
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = await _expenseRepository.GetByIdAsync(id);
            if (entity is null)
                return new ErrorResult("Silinecek harcama bulunamadı.");

            await _expenseRepository.DeleteAsync(entity);
            await _expenseRepository.SaveChangesAsync();

            // Kullanıcıya bilgilendirme
            if (entity.AppUser != null && !string.IsNullOrWhiteSpace(entity.AppUser.Email))
            {
                await _mailService.SendMailAsync(new MailDTO
                {
                    Email = entity.AppUser.Email,
                    Subject = "Expense Deleted",
                    Message = "Your expense request has been deleted."
                });
            }

            return new SuccessResult("Harcama başarıyla silindi.");
        }

        public async Task<IDataResult<List<ExpenseListDTO>>> GetAllAsync()
        {
            var expenses = await _expenseRepository.GetAllAsync(true); // Sadece tracking parametresi gönderildi  

            // Verileri CreatedDate'e göre sıralıyoruz  
            var sortedExpenses = expenses.OrderBy(x => x.CreatedDate).ToList();

            if (sortedExpenses.Count <= 0)
            {
                return new ErrorDataResult<List<ExpenseListDTO>>(sortedExpenses.Adapt<List<ExpenseListDTO>>(), "Listelenecek harcama bulunamadı.");
            }

            var expenseListDTOs = new List<ExpenseListDTO>();

            foreach (var expense in sortedExpenses)
            {
                var expenseListDTO = expense.Adapt<ExpenseListDTO>();

                // Manager rol bilgisi ve diğer gerekli bilgileri DTO'ya ekleyin  
                expenseListDTO.AppUserDisplayName = expense.AppUser.FirstName;

                // Convert the string Role to the Roles enum  
                if (Enum.TryParse<Roles>(expense.AppUser.Role, out var role))
                {
                    expenseListDTO.Roles = role;
                }
                else
                {
                    expenseListDTO.Roles = Roles.Employee; // Default value if parsing fails  
                }

                expenseListDTOs.Add(expenseListDTO);
            }

            return new SuccessDataResult<List<ExpenseListDTO>>(expenseListDTOs, "Harcama listeleme başarılı.");
        }

        public async Task<IDataResult<ExpenseDTO>> GetByIdAsync(Guid id)
        {
            var entity = await _expenseRepository.GetByIdAsync(id);
            if (entity is null)
                return new ErrorDataResult<ExpenseDTO>("Gösterilecek harcama bulunamadı.");

            return new SuccessDataResult<ExpenseDTO>(entity.Adapt<ExpenseDTO>(), "Harcama gösterme işlemi başarılı.");
        }

        public async Task<IDataResult<ExpenseDTO>> UpdateAsync(ExpenseUpdateDTO dto)
        {
            var entity = await _expenseRepository.GetByIdAsync(dto.Id);
            if (entity is null)
                return new ErrorDataResult<ExpenseDTO>("Güncellenecek harcama bulunamadı.");

            entity.Amount = dto.Amount;
            entity.ExpenseDate = dto.ExpenseDate;
            entity.Description = dto.Description ?? entity.Description;
            entity.AppUserId = dto.AppUserId;

            if (dto.Image != null && dto.Image.Length > 0)
                entity.Image = dto.Image;

            try
            {
                await _expenseRepository.UpdateAsync(entity);
                await _expenseRepository.SaveChangesAsync();
                return new SuccessDataResult<ExpenseDTO>(entity.Adapt<ExpenseDTO>(), "Harcama güncelleme başarılı.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Harcama güncellenirken bir hata oluştu.");
                return new ErrorDataResult<ExpenseDTO>("Harcama güncelleme sırasında bir hata oluştu.");
            }
        }
    }
}

