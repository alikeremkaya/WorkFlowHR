using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.UI.Areas.Employee.Models.LeaveVMs
{
    public class LeaveUpdateVM
    {
        public Guid Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        public Guid LeaveTypeId { get; set; }
        public LeaveStatus LeaveStatus { get; set; }
        public Guid ManagerId { get; set; }
        public SelectList LeaveTypes { get; set; }
        public Guid AppUserId { get; set; }
        public SelectList? Managers { get; set; }
    }
}
