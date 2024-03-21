namespace VisualVibes.Domain.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Feed UserFeed { get; set; }
    }
}
