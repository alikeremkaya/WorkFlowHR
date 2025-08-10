


using System.Data;
using WorkFlowHR.Domain.Entities;
using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.Application.DTOs.LeaveTypeDTOs
{
    public class LeaveTypeListDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public LeaveType LeaveTypeId { get; set; }
        public LeaveStatus LeaveStatus { get; set; }
        public Roles Roles { get; set; }

    }
}
