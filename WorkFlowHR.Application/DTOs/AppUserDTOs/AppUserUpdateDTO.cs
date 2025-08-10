

namespace WorkFlowHR.Application.DTOs.AppUserDTOs
{
    public class AppUserUpdateDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? DisplayName { get; set; }
        public string Role { get; set; } = null!;
    }
}
