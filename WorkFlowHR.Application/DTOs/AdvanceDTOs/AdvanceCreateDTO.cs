

namespace WorkFlowHR.Application.DTOs.AdvanceDTOs
{
    public class AdvanceCreateDTO
    {
        public double Amount { get; set; }                
        public DateTime AdvanceDate { get; set; }
        public string Description { get; set; } = null!;
        public byte[]? Image { get; set; }

        public string AppUserName { get; set; } = "";
        public Guid AppUserId { get; set; }

       
    }
}
