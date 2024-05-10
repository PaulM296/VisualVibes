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

        public async Task AddPostToFeedAsync(Guid postId)
        {
            var post = await _context.Posts.Include(p => p.User.Followers)
                        .FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null)
            {
                throw new EntityNotFoundException($"Post with ID {postId} not found.");
            }

            foreach (var follower in post.User.Followers)
            {
                var feed = await _context.Feeds.FirstOrDefaultAsync(f => f.UserID == follower.FollowerId);
                if (feed == null)
                {
                    feed = new Feed { UserID = follower.FollowerId };
                    _context.Feeds.Add(feed);
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"Feed created for follower {follower.FollowerId}");
                }

                // Check if the FeedPost already exists
                var feedPostExists = await _context.FeedPost.AnyAsync(fp => fp.FeedId == feed.Id && fp.PostId == postId);
                if (!feedPostExists)
                {
                    _context.FeedPost.Add(new FeedPost
                    {
                        FeedId = feed.Id,
                        PostId = postId,
                    });
                    Console.WriteLine($"Adding post {postId} to feed {feed.Id} of follower {follower.FollowerId}.");
                }
            }

            await _context.SaveChangesAsync();
            Console.WriteLine("Changes to FeedPost saved successfully.");
        }

        public async Task EnsureFeedForUserAsync(Guid userId)
        {
            var feed = await _context.Feeds.FirstOrDefaultAsync(f => f.UserID == userId);
            if (feed == null)
            {
                feed = new Feed { UserID = userId };
                _context.Feeds.Add(feed);
                await _context.SaveChangesAsync();
            }

            
            if (!await _context.FeedPost.AnyAsync(fp => fp.FeedId == feed.Id))
            {
                
                var topPosts = await _context.Posts
                    .OrderByDescending(p => p.Reactions.Count)
                    .Take(20)
                    .ToListAsync();

                foreach (var post in topPosts)
                {
                    _context.FeedPost.Add(new FeedPost { FeedId = feed.Id, PostId = post.Id });
                }

                await _context.SaveChangesAsync();
            }
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

        public async Task<IEnumerable<FeedPost>> GetByFeedIdAsync(Guid feedId)
        {
            return await _context.FeedPost
                                 .Where(fp => fp.FeedId == feedId)
                                 .Include(fp => fp.Post)
                                 .ToListAsync();
        }

        public async Task RemoveAsync(FeedPost feedPost)
        {
            if (feedPost == null)
            {
                throw new ArgumentNullException(nameof(feedPost), "Provided feed post is null");
            }

            _context.FeedPost.Remove(feedPost);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<FeedPost>> GetByPostIdAsync(Guid postId)
        {
            return await _context.FeedPost
                                 .Where(fp => fp.PostId == postId)
                                 .ToListAsync();
        }
    }
}
