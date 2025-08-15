

using WorkFlowHR.Application.DTOs.ExpenseDTOs;
using WorkFlowHR.Domain.Utilities.Interfaces;

namespace WorkFlowHR.Application.Services.ExpenseServices
{
    public interface IExpenseService
    {
        Task<IDataResult<List<ExpenseListDTO>>> GetAllAsync();
        Task<IDataResult<ExpenseDTO>> CreateAsync(ExpenseCreateDTO expenseCreateDTO);
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<ExpenseDTO>> GetByIdAsync(Guid id);
        Task<IDataResult<ExpenseDTO>> UpdateAsync(ExpenseUpdateDTO expenseUpdateDTO);

    }
}
