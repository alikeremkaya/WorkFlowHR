using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.UI.Areas.Manager.Models.EmployeeVMs
{
    public class EmployeeListVM
    {
        public Guid Id { get; set; }
        public string AppUserDisplayName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] Image { get; set; }
        public string Email { get; set; }
        
        public Roles Roles { get; set; }
    }
}
