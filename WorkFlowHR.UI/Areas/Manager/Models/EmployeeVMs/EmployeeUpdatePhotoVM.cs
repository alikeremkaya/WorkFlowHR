namespace WorkFlowHR.UI.Areas.Manager.Models.EmployeeVMs
{
    public class EmployeeUpdatePhotoVM
    {
        public Guid Id { get; set; }
        public string? DisplayName { get; set; }
        public string Email { get; set; } = default!;
        public byte[]? ExistingImage { get; set; }
        public IFormFile? NewImage { get; set; }
    }
}
