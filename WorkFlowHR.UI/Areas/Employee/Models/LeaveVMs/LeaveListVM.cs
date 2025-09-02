using System.ComponentModel.DataAnnotations;
using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.UI.Areas.Employee.Models.LeaveVMs
{
    public class LeaveListVM
    {
        public Guid Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        public LeaveStatus LeaveStatus { get; set; }

        public string LeaveTypeName { get; set; }



        public string AppUserDisplayName { get; set; }

        public string Description { get; set; }


        public Roles Roles { get; set; }
    }
}
