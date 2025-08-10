namespace WorkFlowHR.UI.Areas.Manager.Models.AppUserVMs
{
    public class AppUserListVM
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string AzureAdObjectId { get; set; } = null!;
    }
}
