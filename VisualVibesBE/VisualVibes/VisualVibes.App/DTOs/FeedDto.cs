using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.DTOs
{
    public class FeedDto
    {
        public Guid Id { get; set; }
        public List<PostDto> Posts { get; set; }

        public static FeedDto FromFeed(Feed feed)
        {
            return new FeedDto 
            { 
                //Id = feed.Id, 
                //Posts = feed.Posts.Select(p => PostDto.FromPost(p)).ToList()
            };
        }
    }
}
