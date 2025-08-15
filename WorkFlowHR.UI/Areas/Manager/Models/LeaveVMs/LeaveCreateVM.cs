using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.UI.Areas.Manager.Models.LeaveVMs
{
    public class LeaveCreateVM
    {
       
        public DateTime StartDate { get; set; } = DateTime.Now;
       
        public DateTime EndDate { get; set; } = DateTime.Now;
        public Roles Roles { get; set; }

        public Guid LeaveTypeId { get; set; }

        public SelectList LeaveTypes { get; set; } // controller doldurur

        public Guid AppUserId { get; set; }
        public SelectList AppUser { get; set; }

        public Guid? ManagerAppUserId { get; set; }        // opsiyonel
        public SelectList? Managers { get; set; }          // controller dolduracak

        public Guid ManagerId { get; set; }
    }
}
