using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WorkFlowHR.UI.Areas.Manager.Models.ExpenseVMs
{
    public class ExpenseEditVM
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime ExpenseDate { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public Domain.Enums.Status? Status { get; set; }

        public Guid? ManagerAppUserId { get; set; }

        // UI: mevcut görsel + yeni görsel
        public byte[]? ExistingImage { get; set; }
        public IFormFile? NewImage { get; set; }

        public SelectList Managers { get; set; }
        public Guid ManagerId { get; set; }
    }
}
