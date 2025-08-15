using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WorkFlowHR.UI.Areas.Manager.Models.ExpenseVMs
{
    public class ExpenseCreateVM
    {
        public double Amount { get; set; }
        public DateTime ExpenseDate { get; set; } = DateTime.Now;
        public IFormFile? NewImage { get; set; }
        public string Description { get; set; }
        public Guid ManagerId { get; set; }
        public string AppUserDisplayName { get; set; }
       
        public SelectList Managers { get; set; }
    }
}
