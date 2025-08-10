
using WorkFlowHR.Domain.Core.Interfaces;

namespace WorkFlowHR.Domain.Core.Base
{
    public class AuditableEntity : BaseEntity, IDeletableEntity
    {
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
