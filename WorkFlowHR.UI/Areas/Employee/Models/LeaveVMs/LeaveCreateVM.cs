﻿using Microsoft.AspNetCore.Mvc.Rendering;
using WorkFlowHR.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace WorkFlowHR.UI.Areas.Employee.Models.LeaveVMs
{
    public class LeaveCreateVM
    {
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        public Roles Roles { get; set; }
        public Guid LeaveTypeId { get; set; }
        public SelectList? LeaveTypes { get; set; }

        public Guid ManagerId { get; set; }
        public SelectList Managers { get; set; }
        public string ManagerFirstName { get; set; }

        public string ManagerLastName { get; set; }
    }
}
