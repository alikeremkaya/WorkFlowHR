using Microsoft.AspNetCore.Mvc.Rendering;
using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.UI.Areas.Manager.Models.AdvanceVMs
{
    // UI/ViewModels/Advance/AdvanceEditVM.cs
    public class AdvanceEditVM
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public DateTime AdvanceDate { get; set; }
      

        public byte[]? ExistingImage { get; set; }         // mevcut resim gösterimi
      

        public IFormFile? NewImage { get; set; }

        public Guid AppUserId { get; set; }                // değişmez
        public Guid? ManagerAppUserId { get; set; }
        public AdvanceStatus AdvanceStatus { get; set; }

        public SelectList? Managers { get; set; }
        public Guid ManagerId { get; set; }
    }

}
