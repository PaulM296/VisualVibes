using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.DTOs.FeedDtos
{
    public class CreateFeedDto
    {
        public Guid UserID { get; set; }

        public static CreateFeedDto FromFeed(Feed feed)
        {
            return new CreateFeedDto
            {
                UserID = feed.UserID
            };
        }
    }
}
