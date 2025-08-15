using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.UI.Areas.Employee.Models.AdvanceVMs
{
    public class AdvanceCreateVM
    {
        public double Amount { get; set; }
        public DateTime AdvanceDate { get; set; } = DateTime.Now;
        public string Description { get; set; } = null!;

        public IFormFile? ImageFile { get; set; }        

        public Guid? ManagerAppUserId { get; set; }     
        public SelectList? Managers { get; set; }

        public string AppUserDisplayName { get; set; } = null!;
    

        public IFormFile NewImage { get; set; }
        public Roles Roles { get; set; }
        public Guid ManagerId { get; set; }

        public AdvanceStatus AdvanceStatus { get; set; } = AdvanceStatus.Pending;
    }
}
