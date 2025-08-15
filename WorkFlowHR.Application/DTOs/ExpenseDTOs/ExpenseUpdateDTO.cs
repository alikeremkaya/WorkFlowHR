

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Data;
using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.Application.DTOs.ExpenseDTOs
{
    public class ExpenseUpdateDTO
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ExpenseDate { get; set; }

        public byte[]? Image { get; set; }

        public string Description { get; set; }

        public Guid AppUserId { get; set; }

        public Roles Roles { get; set; }

    }
}
