using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserProfile UserProfile { get; set; }
        public Feed UserFeed { get; set; }
        public List<User> Followers { get; set; }
        public List<User> Following { get; set; }

        public static UserDto FromUser(User User)
        {
            return new UserDto
            {
                Id = User.Id,
                Username = User.Username,
                Password = User.Password,
                UserProfile = User.UserProfile,
                UserFeed = User.UserFeed,
                Followers = User.Followers,
                Following = User.Following
            };
        }
    }
}
