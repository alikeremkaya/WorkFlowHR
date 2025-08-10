using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.UI.Areas.Manager.Models.LeaveVMs
{
    public class LeaveDetailsVM
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LeaveType { get; set; } = null!;
        public LeaveStatus LeaveStatus { get; set; }
    }
}
