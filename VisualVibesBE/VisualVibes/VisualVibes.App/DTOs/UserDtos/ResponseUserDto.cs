using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.DTOs.UserDtos
{
    public class ResponseUserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public static ResponseUserDto FromUser(User User)
        {
            return new ResponseUserDto
            {
                Id = User.Id,
                Username = User.Username,
                Password = User.Password,
            };
        }
    }
}
