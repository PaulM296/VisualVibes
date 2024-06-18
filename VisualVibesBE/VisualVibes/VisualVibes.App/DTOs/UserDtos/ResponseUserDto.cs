using VisualVibes.Domain.Enum;

    namespace VisualVibes.App.DTOs.UserDtos
    {
        public class ResponseUserDto
        {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePicture { get; set; }
        public Guid ImageId { get; set; }
        public bool isBlocked { get; set; }
    }
}
