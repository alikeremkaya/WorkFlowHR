

using System.ComponentModel.DataAnnotations;
using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.Application.DTOs.ExpenseDTOs
{
    public class ExpenseListDTO
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ExpenseDate { get; set; }

        public byte[]? Image { get; set; }

        public string Description { get; set; }

        public Guid AppUserId { get; set; }
        public string AppUserDisplayName { get; set; }

        public Roles Roles { get; set; }
    }
}
