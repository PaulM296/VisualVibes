using System.ComponentModel.DataAnnotations;

namespace VisualVibes.Domain.Models.BaseEntity
{
    public class User : BaseEntity
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public UserProfile UserProfile { get; set; }

        public Feed UserFeed { get; set; }

        public ICollection<Conversation> StartedConversations { get; set; }
        public ICollection<Conversation> JoinedConversations { get; set; }

        public ICollection<Post> Posts { get; set; }

        public ICollection<UserFollower> Followers { get; set; }

        public ICollection<UserFollower> Following {  get; set; }

        public ICollection<Reaction> Reactions { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}
