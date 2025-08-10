

namespace WorkFlowHR.Application.DTOs.AppUserDTOs;

public class AppUserCreateDTO
{
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string AzureAdObjectId { get; set; } = null!;
}
