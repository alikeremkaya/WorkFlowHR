using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowHR.Domain.Enums;

namespace WorkFlowHR.Application.DTOs.AdvanceDTOs
{
    public class AdvanceListDTO
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime AdvanceDate { get; set; }
       
        public byte[] Image { get; set; }
        public Roles Roles { get; set; }
        public Guid? ManagerAppUserId { get; set; }
        public Guid? AppUserId { get; set; }
        public AdvanceStatus AdvanceStatus { get; set; } = AdvanceStatus.Pending;
        public string AppUserDisplayName { get; set; } = null!;

        





    }
}
