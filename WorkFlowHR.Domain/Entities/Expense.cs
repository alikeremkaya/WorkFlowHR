using WorkFlowHR.Domain.Core.Base;

namespace WorkFlowHR.Domain.Entities
{
    public class Expense:AuditableEntity
    {
        public double Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public byte[]? Image { get; set; }
        public string Description { get; set; } = null!;

        // İlgili kullanıcı (AppUser) ilişkisi
        public Guid? ManagerAppUserId { get; set; }
        public virtual AppUser? ManagerAppUser { get; set; }
    }
}
