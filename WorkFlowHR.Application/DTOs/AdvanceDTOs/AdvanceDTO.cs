using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.Application.DTOs.AdvanceDTOs
{
    public class AdvanceDTO
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public DateTime AdvanceDate { get; set; }
        public string Description { get; set; } = null!;
        public byte[]? Image { get; set; }

        public Guid AppUserId { get; set; }
        public string AppUserName { get; set; } = "";      // AppUser.DisplayName veya email prefix

        public Guid? ManagerAppUserId { get; set; }
        public string? ManagerName { get; set; }

        public AdvanceStatus AdvanceStatus { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
