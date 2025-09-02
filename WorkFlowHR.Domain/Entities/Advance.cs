

using WorkFlowHR.Domain.Core.Base;
using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.Domain.Entities
{
    public class Advance:AuditableEntity
    {
        public decimal Amount { get; set; }
        public DateTime AdvanceDate { get; set; }
    
        public byte[]? Image { get; set; }
        public AdvanceStatus AdvanceStatus { get; set; } = AdvanceStatus.Pending;

        public Guid AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; } = null!;
       

    }
}
