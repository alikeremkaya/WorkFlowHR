using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.UI.Areas.Employee.Models.AdvanceVMs
{
    public class AdvanceCreateVM
    {
        public double Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime AdvanceDate { get; set; }
        public IFormFile NewImage { get; set; }
        public Roles Roles { get; set; }
        public Guid ManagerId { get; set; }
        public SelectList Managers { get; set; }
        public AdvanceStatus AdvanceStatus { get; set; } = AdvanceStatus.Pending;

         public string ManagerFirstName { get; set; }

        public string ManagerLastName { get; set; }
    }
}
