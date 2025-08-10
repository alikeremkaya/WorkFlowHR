

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace WorkFlowHR.Application.DTOs.ExpenseDTOs
{
    public class ExpenseUpdateDTO
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

        // Yeni resim yüklenirse byte[] olarak gönderilir
        public byte[]? Image { get; set; }

    }
}
