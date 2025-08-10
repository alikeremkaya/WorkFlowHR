namespace WorkFlowHR.UI.Areas.Manager.Models.LeaveTypeVMs
{
    public class LeaveTypeEditVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
