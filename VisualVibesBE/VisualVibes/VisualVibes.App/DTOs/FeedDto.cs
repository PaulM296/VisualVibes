using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.DTOs
{
    public class FeedDto
    {
        public Guid Id { get; set; }
        public Guid UserID { get; set; }

        public static FeedDto FromFeed(Feed feed)
        {
            return new FeedDto 
            {
                Id = feed.Id,
                UserID = feed.UserID
            };
        }
    }
}
