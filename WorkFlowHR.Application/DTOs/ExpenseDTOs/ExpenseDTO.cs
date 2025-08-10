

using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.Application.DTOs.ExpenseDTOs
{
    public class ExpenseDTO
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string? Description { get; set; }

        public Guid AppUserId { get; set; }
        public string AppUserName { get; set; } = "";

        public DateTime CreatedDate { get; set; }

        public Guid? UpdatedById { get; set; }
        public string? UpdatedByName { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public Guid? ManagerAppUserId { get; set; }
        public string? ManagerName { get; set; }

        public Status Status { get; set; }

        public byte[]? Image { get; set; }
    }
}
