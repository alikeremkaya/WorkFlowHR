

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WorkFlowHR.Application.DTOs.ExpenseDTOs
{
    public class ExpenseCreateDTO
    {
        [Required]
        public double Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ExpenseDate { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
        public IFormFile? NewImage { get; set; }
        public Guid AppUserId { get; set; }

        public string AppUserDisplayName { get; set; }

        // Onaylayacak yönetici


        public byte[]? Image { get; set; }

      
      


    }
}
