

namespace WorkFlowHR.Application.DTOs.LeaveTypeDTOs
{
    public class LeaveTypeEditDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
