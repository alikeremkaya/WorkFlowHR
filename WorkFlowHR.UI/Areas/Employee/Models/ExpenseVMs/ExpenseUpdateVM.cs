﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace WorkFlowHR.UI.Areas.Employee.Models.ExpenseVMs
{
    public class ExpenseUpdateVM
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }

        public DateTime ExpenseDate { get; set; }
        public IFormFile? NewImage { get; set; }
        public byte[]? ExistingImage { get; set; }
        public string Description { get; set; }
        public Guid ManagerId { get; set; }
        public string ManagerFirstName { get; set; }
        public string ManagerLastName { get; set; }
        public SelectList Managers { get; set; }
    }
}
