using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.UI.Areas.Manager.Models.ExpenseVMs
{
    public class ExpenseListVM
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public Guid AppUserId { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public string DisplayName { get; set; } = "";
        public string Email { get; set; } = "";

        public DateTime CreatedDate { get; set; }
        public Domain.Enums.Status Status { get; set; }
        public string Role { get; set; } = "";
    }
}
