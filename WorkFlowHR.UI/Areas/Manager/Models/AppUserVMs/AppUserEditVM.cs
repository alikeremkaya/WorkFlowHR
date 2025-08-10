namespace WorkFlowHR.UI.Areas.Manager.Models.AppUserVMs
{
    public class AppUserEditVM
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? DisplayName { get; set; }
        public string Role { get; set; } = null!;
        public string? AzureAdObjectId { get; set; }

      


    }
}
