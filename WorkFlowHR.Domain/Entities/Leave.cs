

using WorkFlowHR.Domain.Core.Base;
using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.Domain.Entities
{
    public class Leave:AuditableEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveStatus LeaveStatus { get; set; } = LeaveStatus.Pending;


        // Zorunlu: tür
        public Guid LeaveTypeId { get; set; }
        public virtual LeaveType LeaveType { get; set; } = null!;

        // Zorunlu: kaydı oluşturan kullanıcı (login olan)
        public Guid AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; } = null!;

    
       
    }
}
