

using WorkFlowHR.Application.DTOs.ExpenseDTOs;
using WorkFlowHR.Domain.Utilities.Interfaces;

namespace WorkFlowHR.Application.Services.ExpenseServices
{
    public interface IExpenseService
    {
        Task<IDataResult<List<ExpenseListDTO>>> GetAllAsync();
        Task<IDataResult<ExpenseDTO>> CreateAsync(ExpenseCreateDTO dto);
        Task<IDataResult<ExpenseDTO>> GetByIdAsync(Guid id);
        Task<IResult> UpdateAsync(ExpenseUpdateDTO dto);
        Task<IResult> DeleteAsync(Guid id);

    }
}
