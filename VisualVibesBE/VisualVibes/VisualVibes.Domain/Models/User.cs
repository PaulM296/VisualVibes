namespace VisualVibes.Domain.Models.BaseEntity
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public UserProfile UserProfile { get; set; }
        public Feed UserFeed { get; set; }
        public List<User> Followers { get; set; }
        public List<User> Following {  get; set; }
    }
}
