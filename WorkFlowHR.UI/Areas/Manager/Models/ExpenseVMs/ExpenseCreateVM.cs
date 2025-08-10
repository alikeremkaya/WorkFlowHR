using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WorkFlowHR.UI.Areas.Manager.Models.ExpenseVMs
{
    public class ExpenseCreateVM
    {
        [Required]
        public double Amount { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime ExpenseDate { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string? Description { get; set; }

        // Onaylayacak yönetici
        public Guid? ManagerAppUserId { get; set; }

        // UI: yükleme için
        public IFormFile? ImageFile { get; set; }

        public IFormFile? NewImage { get; set; }
        public Guid ManagerId { get; set; }

        // DTO’da da var; controller GET/invalid POST’ta doldur
        public SelectList Managers { get; set; }
    }
}
