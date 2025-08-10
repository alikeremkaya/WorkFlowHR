namespace WorkFlowHR.UI.Areas.Manager.Views.AppUser
{
    public class AppUserCreateVM
    {
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string AzureAdObjectId { get; set; } = null!;
    }
}
