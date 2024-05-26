using VisualVibes.App.Interfaces;

namespace VisualVibes.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VisualVibesDbContext _context;
        public ICommentRepository CommentRepository { get; private set; }
        public IConversationRepository ConversationRepository { get; private set; }
        public IFeedRepository FeedRepository { get; private set; }
        public IMessageRepository MessageRepository { get; private set; }
        public IPostRepository PostRepository { get; private set; }
        public IReactionRepository ReactionRepository { get; private set; }
        public IUserProfileRepository UserProfileRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }
        public IUserFollowerRepository UserFollowerRepository { get; private set; }
        public IFeedPostRepository FeedPostRepository { get; private set; }
        public IImageRepository ImageRepository { get; private set; }

        public UnitOfWork(VisualVibesDbContext context, ICommentRepository commentRepository,
            IConversationRepository conversationRepository, IFeedRepository feedRepository, IMessageRepository messageRepository,
            IPostRepository postRepository, IReactionRepository reactionRepository, IUserRepository userRepository, 
            IUserProfileRepository userProfileRepository, IUserFollowerRepository userFollowerRepository, 
            IFeedPostRepository feedPostRepository, IImageRepository imageRepository)
        {
            _context = context;
            CommentRepository = commentRepository;
            ConversationRepository = conversationRepository;
            FeedRepository = feedRepository;
            MessageRepository = messageRepository;
            PostRepository = postRepository;
            ReactionRepository = reactionRepository;
            UserRepository = userRepository;
            UserProfileRepository = userProfileRepository;
            UserFollowerRepository = userFollowerRepository;
            FeedPostRepository = feedPostRepository;
            ImageRepository = imageRepository;
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
