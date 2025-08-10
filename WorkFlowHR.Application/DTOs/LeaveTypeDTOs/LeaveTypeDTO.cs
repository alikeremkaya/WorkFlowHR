

namespace WorkFlowHR.Application.DTOs.LeaveTypeDTOs
{
    public class LeaveTypeDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; }
    }
}
