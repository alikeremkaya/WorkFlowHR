using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.UI.Areas.Manager.Models.AdvanceVMs
{
    public class AdvanceEditVM
    {
        public Guid Id { get; set; }

        public double Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime AdvanceDate { get; set; }

        public IFormFile NewImage { get; set; }

        public byte[] ExistingImage { get; set; }
        public Guid ManagerId { get; set; }
        public SelectList Managers { get; set; }
        public AdvanceStatus AdvanceStatus { get; set; } = AdvanceStatus.Pending;
        public string AppUserDisplayName { get; set; } = null!;
    }
}
