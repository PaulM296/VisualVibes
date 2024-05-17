//using MediatR;
//using Microsoft.Extensions.Logging;
//using VisualVibes.App.Exceptions;
//using VisualVibes.App.Interfaces;
//using VisualVibes.App.Users.Commands;

//namespace VisualVibes.App.Users.CommandsHandler
//{
//    public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand, Unit>
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly ILogger<RemoveUserCommandHandler> _logger;

//        public RemoveUserCommandHandler(IUnitOfWork unitOfWork, ILogger<RemoveUserCommandHandler> logger)
//        {
//            _unitOfWork = unitOfWork;
//            _logger = logger;
//        }

//        public async Task<Unit> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
//        {
//            var userToRemove = await _unitOfWork.UserRepository.GetUserByIdAsync(request.Id);


//            if (userToRemove == null)
//            {
//                throw new UserNotFoundException($"The user with ID {request.Id} doesn't exist and it could not be removed!");
//            }

//            var posts = await _unitOfWork.PostRepository.GetPostsByUserIdAsync(request.Id);

//            foreach (var post in posts)
//            {
//                var comments = await _unitOfWork.CommentRepository.GetAllAsync(post.Id);
//                foreach (var comment in comments)
//                {
//                    await _unitOfWork.CommentRepository.RemoveAsync(comment);
//                }

//                var reactions = await _unitOfWork.ReactionRepository.GetAllAsync(post.Id);
//                foreach (var reaction in reactions)
//                {
//                    await _unitOfWork.ReactionRepository.RemoveAsync(reaction);
//                }

//                await _unitOfWork.PostRepository.RemoveAsync(post);
//            }

//            var followers = await _unitOfWork.UserFollowerRepository.GetFollowersByUserIdAsync(request.Id);
//            var followings = await _unitOfWork.UserFollowerRepository.GetFollowingByUserIdAsync(request.Id);

//            foreach (var follower in followers)
//            {
//                await _unitOfWork.UserFollowerRepository.RemoveFollowerAsync(follower.FollowerId, follower.FollowingId);
//            }

//            foreach (var following in followings)
//            {
//                await _unitOfWork.UserFollowerRepository.RemoveFollowerAsync(following.FollowerId, following.FollowingId);
//            }

//            var conversations = await _unitOfWork.ConversationRepository.GetAllByUserIdAsync(request.Id);
//            foreach (var conversation in conversations)
//            {
//                var messages = await _unitOfWork.MessageRepository.GetAllAsync(conversation.Id);
//                foreach(var message in messages)
//                {
//                    await _unitOfWork.MessageRepository.RemoveAsync(message);
//                }

//                await _unitOfWork.ConversationRepository.RemoveAsync(conversation);
//            }

//            var feed = await _unitOfWork.FeedRepository.GetByUserIdAsync(request.Id);
//            if (feed != null)
//            {
//                var feedPosts = await _unitOfWork.FeedPostRepository.GetByFeedIdAsync(feed.Id);
//                foreach (var feedPost in feedPosts)
//                {
//                    await _unitOfWork.FeedPostRepository.RemoveAsync(feedPost);
//                }

//                await _unitOfWork.FeedRepository.RemoveAsync(feed);
//            }

//            await _unitOfWork.UserRepository.RemoveAsync(userToRemove);
//            await _unitOfWork.SaveAsync();

//            _logger.LogInformation("User succesfully !");

//            return Unit.Value;
//        }
//    }
//}
