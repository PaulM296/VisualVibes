using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Domain.Models
{
    public class FeedPost
    {
        public Guid FeedId { get; set; }
        public Feed Feed { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}
