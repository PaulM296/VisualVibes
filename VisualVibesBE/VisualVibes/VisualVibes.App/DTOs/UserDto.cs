using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<UserDto> Followers { get; set; }
        public List<UserDto> Following { get; set; }

        public static UserDto FromUser(User User)
        {
            return new UserDto
            {
                Id = User.Id,
                Username = User.Username,
                Password = User.Password,
                Followers = User.Followers.Select(u => FromUser(u)).ToList(),
                Following = User.Following.Select(u => FromUser(u)).ToList()
            };
        }
    }
}
