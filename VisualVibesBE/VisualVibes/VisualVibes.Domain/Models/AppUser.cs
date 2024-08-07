﻿using Microsoft.AspNetCore.Identity;
using VisualVibes.Domain.Enum;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Domain.Models
{
    public class AppUser : IdentityUser
    {
        public Role Role { get; set; }
        public UserProfile UserProfile { get; set; }

        public Feed UserFeed { get; set; }

        public ICollection<Conversation> StartedConversations { get; set; }
        public ICollection<Conversation> JoinedConversations { get; set; }

        public ICollection<Post> Posts { get; set; }

        public ICollection<UserFollower> Followers { get; set; }

        public ICollection<UserFollower> Following { get; set; }

        public ICollection<Reaction> Reactions { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Message> Messages { get; set; }
        public bool isBlocked { get; set; }
    }
}
