using WorkFlowHR.Application.DTOs.AdvanceDTOs;
using WorkFlowHR.Application.DTOs.ExpenseDTOs;
using WorkFlowHR.Application.DTOs.LeaveDTOs;

namespace WorkFlowHR.UI.Areas.Manager.Models.SummaryVMs
{
    public class SummaryViewModel
    {
        public List<AdvanceListDTO> Advances { get; set; }
        public List<ExpenseListDTO> Expenses { get; set; }
        public List<LeaveListDTO> Leaves { get; set; }
    }
}
