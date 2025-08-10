using Mapster;
using Microsoft.Extensions.Logging;
using WorkFlowHR.Application.DTOs.ExpenseDTOs;
using WorkFlowHR.Domain.Entities;
using WorkFlowHR.Domain.Utilities.Concretes;
using WorkFlowHR.Domain.Utilities.Interfaces;
using WorkFlowHR.Infrastructure.Repositories.ExpenseRepositories;

namespace WorkFlowHR.Application.Services.ExpenseServices
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _repo;
        private readonly ILogger<ExpenseService> _logger;

        public ExpenseService(IExpenseRepository repo, ILogger<ExpenseService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<IDataResult<List<ExpenseListDTO>>> GetAllAsync()
        {
            try
            {
                var entities = await _repo.GetAllAsync(e => true, tracking: false);
                var dtos = entities.Adapt<List<ExpenseListDTO>>();
                return new SuccessDataResult<List<ExpenseListDTO>>(dtos, "Expenses retrieved.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting expenses");
                return new ErrorDataResult<List<ExpenseListDTO>>("Failed to retrieve expenses.");
            }
        }

        public async Task<IDataResult<ExpenseDTO>> CreateAsync(ExpenseCreateDTO dto)
        {
            try
            {
                var entity = dto.Adapt<Expense>();
                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync();
                var resultDto = entity.Adapt<ExpenseDTO>();
                return new SuccessDataResult<ExpenseDTO>(resultDto, "Expense created.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating expense");
                return new ErrorDataResult<ExpenseDTO>("Failed to create expense.");
            }
        }

        public async Task<IDataResult<ExpenseDTO>> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
            {
            
                return new ErrorDataResult<ExpenseDTO>("Expense not found.");
               
            }

            var dto = entity.Adapt<ExpenseDTO>();
            return new SuccessDataResult<ExpenseDTO>(dto,"expense found");
        }

        public async Task<IResult> UpdateAsync(ExpenseUpdateDTO dto)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(dto.Id);
                if (entity == null)
                    return new ErrorResult("Expense not found.");

                dto.Adapt(entity);
                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync();
                return new SuccessResult("Expense updated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating expense");
                return new ErrorResult("Failed to update expense.");
            }
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                return new ErrorResult("Expense not found.");

            await _repo.DeleteAsync(entity);
            await _repo.SaveChangesAsync();
            return new SuccessResult("Expense deleted.");
        }




    }
}
