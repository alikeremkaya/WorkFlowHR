

namespace WorkFlowHR.Application.DTOs.AppUserDTOs
{
    public class AppUserListDTO
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
