using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.UI.Areas.Manager.Models.LeaveVMs
{
    public class LeaveListVM
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LeaveType { get; set; } = null!;
        public LeaveStatus LeaveStatus { get; set; }
        public string LeaveTypeName { get; set; }
        public string AppUserDisplayName { get; set; }
        public string AppUserRole { get; set; }
        

    }
}
