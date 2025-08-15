using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.Application.DTOs.LeaveDTOs
{
    public class LeaveListDTO
    {
        public Guid Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        public string LeaveType { get; set; } = null!;
        public LeaveStatus LeaveStatus { get; set; }
        public string LeaveTypeName { get; set; }
        public string AppUserDisplayName { get; set; }
        public string AppUserRole { get; set; }
        public Guid AppUserId { get; set; }

    }
}
