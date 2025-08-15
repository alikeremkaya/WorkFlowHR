using System.ComponentModel.DataAnnotations;
using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.UI.Areas.Manager.Models.AdvanceVMs
{
    public class AdvanceDetailsVM
    {
        public Guid Id { get; set; }

        public double Amount { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime AdvanceDate { get; set; }

        public AdvanceStatus AdvanceStatus { get; set; }

        public byte[] Image { get; set; }

        public string Description { get; set; } // Eğer avans talebiyle ilgili açıklama varsa

        public string AppUserDisplayName { get; set; }

        public Roles Roles { get; set; }
    }
}
