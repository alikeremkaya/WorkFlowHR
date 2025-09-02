using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowHR.Application.DTOs.LeaveDTOs
{
    public class LeaveCreateDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid LeaveTypeId { get; set; }

       
        public Guid AppUserId { get; set; }

        public Guid? ApproverAppUserId { get; set; } 

        public Guid? ManagerAppUserId { get; set; }        
        public SelectList? Managers { get; set; }         
        public Guid ManagerId { get; set; }
    }
}
