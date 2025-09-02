
using WorkFlowHR.Domain.Core.Base;

namespace WorkFlowHR.Domain.Entities
{
    public class LeaveType:AuditableEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<Leave> Leaves { get; set; } = new HashSet<Leave>();
    }
}
