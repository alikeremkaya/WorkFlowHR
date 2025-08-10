

namespace WorkFlowHR.Domain.Core.Base
{
    public class BaseUser : AuditableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
       
    }
}
