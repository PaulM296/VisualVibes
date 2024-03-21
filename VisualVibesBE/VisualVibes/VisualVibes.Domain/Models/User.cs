namespace VisualVibes.Domain.Models.BaseEntity
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Feed UserFeed { get; set; }
    }
}
