using WorkFlowHR.Domain.Entities;
using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.UI.Areas.Manager.Models.LeaveTypeVMs
{
    public class LeaveTypeListVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        // Aşağıdakiler DTO’n ile birebir olsun diye eklendi (tavsiye etmem)
        public LeaveType LeaveTypeId { get; set; }
        public LeaveStatus LeaveStatus { get; set; }
        public Roles Roles { get; set; }
    }
}
