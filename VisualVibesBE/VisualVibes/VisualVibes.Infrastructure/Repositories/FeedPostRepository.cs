using Microsoft.EntityFrameworkCore;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;
using VisualVibes.Domain.Models;
using VisualVibes.Infrastructure.Exceptions;

namespace VisualVibes.Infrastructure.Repositories
{
    public class FeedPostRepository : IFeedPostRepository
    {
        private readonly VisualVibesDbContext _context;

        public FeedPostRepository(VisualVibesDbContext context)
        {
            _context = context;
        }
        //public async Task AddPostToFeedAsync(Guid postId)
        //{
        //    var post = await _context.Posts.Include(p => p.User.Followers)
        //                .FirstOrDefaultAsync(p => p.Id == postId);

        //    if (post == null)
        //    {
        //        throw new EntityNotFoundException($"Post with ID {postId} not found.");
        //    }

        //    var followerIds = post.User.Followers
        //    .Select(f => f.FollowerId).ToList();

        //    foreach (var followerId in followerIds)
        //    {
        //        var feed = await _context.Feeds.FirstOrDefaultAsync(f => f.UserID == followerId);
        //        if (feed != null)
        //        {
        //            _context.FeedPost.Add(new FeedPost
        //            {
        //                FeedId = feed.Id,
        //                PostId = postId,
        //            });
        //        }
        //    }

        //    await _context.SaveChangesAsync();
        //}

        public async Task AddPostToFeedAsync(Guid postId)
        {
            var post = await _context.Posts.Include(p => p.User.Followers)
                        .FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null)
            {
                throw new EntityNotFoundException($"Post with ID {postId} not found.");
            }

            // Check if each follower has a feed and create one if it doesn't exist
            foreach (var follower in post.User.Followers)
            {
                var feed = await _context.Feeds.FirstOrDefaultAsync(f => f.UserID == follower.FollowerId);
                if (feed == null)
                {
                    // Feed does not exist, create it
                    feed = new Feed { UserID = follower.FollowerId };
                    _context.Feeds.Add(feed);
                    await _context.SaveChangesAsync(); // Ensure the feed is created before proceeding
                    Console.WriteLine($"Feed created for follower {follower.FollowerId}");
                }

                // Now add the post to the feed
                _context.FeedPost.Add(new FeedPost
                {
                    FeedId = feed.Id,
                    PostId = postId,
                });
                Console.WriteLine($"Adding post {postId} to feed {feed.Id} of follower {follower.FollowerId}.");
            }

            await _context.SaveChangesAsync(); // Save changes for adding posts to feeds
            Console.WriteLine("Changes to FeedPost saved successfully.");
        }

        public async Task<IEnumerable<Post>> GetFeedPostsAsync(Guid feedId)
        {
            var posts = await _context.FeedPost
                            .Where(fp => fp.FeedId == feedId)
                            .Select(fp => fp.Post)
                            .OrderByDescending(p => p.CreatedAt)
                            .ToListAsync();

            return posts;
        }
    }
}
