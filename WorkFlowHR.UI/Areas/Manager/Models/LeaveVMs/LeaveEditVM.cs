using Microsoft.AspNetCore.Mvc.Rendering;
using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.UI.Areas.Manager.Models.LeaveVMs
{
    public class LeaveEditVM
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid LeaveTypeId { get; set; }
        public LeaveStatus LeaveStatus { get; set; }
        public Guid ManagerId { get; set; }
        public SelectList LeaveTypes { get; set; }
        public SelectList? Managers { get; set; }          // controller dolduracak
    }
}
