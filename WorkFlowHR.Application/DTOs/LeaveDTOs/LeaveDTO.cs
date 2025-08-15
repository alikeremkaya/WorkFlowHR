using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.Application.DTOs.LeaveDTOs
{
    public class LeaveDTO
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public Guid LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; } = null!;

        public Guid AppUserId { get; set; }
        public string AppUserDisplayName { get; set; } = "";

        public Guid? ApproverAppUserId { get; set; }
        public string? ApproverName { get; set; }

        public LeaveStatus LeaveStatus { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
