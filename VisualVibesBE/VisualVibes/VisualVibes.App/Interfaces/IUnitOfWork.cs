namespace VisualVibes.App.Interfaces
{
    public interface IUnitOfWork
    {
        public ICommentRepository CommentRepository { get; }
        public IConversationRepository ConversationRepository { get; }
        public IFeedRepository FeedRepository { get; }
        public IMessageRepository MessageRepository { get; }
        public IPostRepository PostRepository { get; }
        public IReactionRepository ReactionRepository { get; }
        public IUserRepository UserRepository { get; }
        public IUserProfileRepository UserProfileRepository { get; }
        public IUserFollowerRepository UserFollowerRepository { get; }

        Task SaveAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
