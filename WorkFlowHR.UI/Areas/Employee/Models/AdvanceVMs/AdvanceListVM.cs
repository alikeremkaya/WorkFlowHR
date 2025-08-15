using System.ComponentModel.DataAnnotations;
using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.UI.Areas.Employee.Models.AdvanceVMs
{
    public class AdvanceListVM
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime AdvanceDate { get; set; }
        public byte[] Image { get; set; }
        public Roles Roles { get; set; }
        public Guid? ManagerAppUserId { get; set; }
        public AdvanceStatus AdvanceStatus { get; set; } = AdvanceStatus.Pending;

        public string AppUserDisplayName { get; set; } = null!;
     
    }
}
