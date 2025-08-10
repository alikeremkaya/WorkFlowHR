using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.Application.DTOs.LeaveDTOs
{
    public class LeaveUpdateDTO
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid LeaveTypeId { get; set; }
        public LeaveStatus LeaveStatus { get; set; }
        public Guid AppUserId { get; set; }
        public Guid? ApproverAppUserId { get; set; }
    }
}
