

namespace WorkFlowHR.Application.DTOs.ExpenseDTOs
{
    public class ExpenseListDTO
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public DateTime ExpenseDate { get; set; }

        public Guid AppUserId { get; set; }

        // Yeni eklenen alanlar:
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public string DisplayName { get; set; } = "";
        public string Email { get; set; } = "";

        public DateTime CreatedDate { get; set; }
        public Domain.Enums.Status Status { get; set; }
        public string Role { get; set; } = "";
    }
}
