
using Microsoft.AspNetCore.Mvc.Rendering;

using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.Application.DTOs.AdvanceDTOs
{
    public class AdvanceUpdateDTO
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime AdvanceDate { get; set; }
        public string Description { get; set; } = null!;
        public byte[]? Image { get; set; }
        public byte[] ExistingImage { get; set; }

        public Guid AppUserId { get; set; }
        public Guid? ManagerAppUserId { get; set; }
        public SelectList Managers { get; set; }

        public AdvanceStatus AdvanceStatus { get; set; }  
    }
}
