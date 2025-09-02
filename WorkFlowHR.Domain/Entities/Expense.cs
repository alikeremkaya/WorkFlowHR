using WorkFlowHR.Domain.Core.Base;

namespace WorkFlowHR.Domain.Entities
{
    public class Expense:AuditableEntity
    {
        public double Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public byte[]? Image { get; set; }
        public string Description { get; set; } = null!;

        public Guid AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; } = null!;
    }
}
