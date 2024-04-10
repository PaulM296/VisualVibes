using Moq;
using VisualVibes.App.Conversations.Queries;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Reactions.Queries;
using VisualVibes.App.Reactions.QueriesHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.Reactions
{
    public class GetAllPostReactionQueryHandlerUnitTest
    {
        private readonly Mock<IReactionRepository> _reactionRepositoryMock;
        private GetAllPostReactionsQueryHandler _getAllPostReactionsQueryHandler;

        public GetAllPostReactionQueryHandlerUnitTest()
        {
            _reactionRepositoryMock = new Mock<IReactionRepository>();
            _getAllPostReactionsQueryHandler = new GetAllPostReactionsQueryHandler(_reactionRepositoryMock.Object);
        }

        [Fact] 
        public async void Should_GetAllPostReactions_Correctly()
        {
            var postId = Guid.NewGuid();

            var reactionDtos = new List<ReactionDto>
            {
                new ReactionDto
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    PostId = postId,
                    ReactionType = Domain.Enum.ReactionType.Love,
                    Timestamp = DateTime.Now,
                },
                new ReactionDto
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    PostId = postId,
                    ReactionType = Domain.Enum.ReactionType.Love,
                    Timestamp = DateTime.Now,
                }
            };

            var reactions = new List<Reaction>();
            foreach (var reactionDto in reactionDtos)
            {
                reactions.Add(new Reaction
                {
                    Id = reactionDto.Id,
                    UserId = reactionDto.UserId,
                    PostId = reactionDto.PostId,
                    ReactionType = reactionDto.ReactionType,
                    Timestamp = reactionDto.Timestamp
                });
            }

            var getAllPostReactionQuery = new GetAllPostReactionsQuery(postId);
            
            _reactionRepositoryMock
            .Setup(x => x.GetAllAsync(postId))
                .ReturnsAsync(reactions);

            //Act
            var result = await _getAllPostReactionsQueryHandler.Handle(getAllPostReactionQuery, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(reactionDtos.Count, result.Count);
        }
    }
}
