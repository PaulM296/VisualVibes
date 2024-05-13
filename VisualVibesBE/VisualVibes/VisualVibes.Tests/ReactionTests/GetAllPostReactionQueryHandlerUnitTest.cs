using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using VisualVibes.App.DTOs.ReactionDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Reactions.Queries;
using VisualVibes.App.Reactions.QueriesHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.ReactionTests
{
    public class GetAllPostReactionQueryHandlerUnitTest
    {
        private readonly Mock<IReactionRepository> _reactionRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<GetAllPostReactionsQueryHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock; 
        private readonly GetAllPostReactionsQueryHandler _getAllPostReactionsQueryHandler;

        public GetAllPostReactionQueryHandlerUnitTest()
        {
            _reactionRepositoryMock = new Mock<IReactionRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<GetAllPostReactionsQueryHandler>>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(uow => uow.ReactionRepository).Returns(_reactionRepositoryMock.Object);
            
            _getAllPostReactionsQueryHandler = new GetAllPostReactionsQueryHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async void Should_GetAllPostReactions_Correctly()
        {
            var postId = Guid.NewGuid();

            var reactions = new List<Reaction>
            {
                new Reaction
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    PostId = postId,
                    ReactionType = Domain.Enum.ReactionType.Love,
                    Timestamp = DateTime.Now,
                },
                new Reaction
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    PostId = postId,
                    ReactionType = Domain.Enum.ReactionType.Like,
                    Timestamp = DateTime.Now,
                }
            };

            var reactionDtos = reactions.Select(r => new ResponseReactionDto
            {
                Id = r.Id,
                UserId = r.UserId,
                PostId = r.PostId,
                ReactionType = r.ReactionType,
                Timestamp = r.Timestamp,
            }).ToList();

            var getAllPostReactionQuery = new GetAllPostReactionsQuery(postId);

            _reactionRepositoryMock
                .Setup(x => x.GetAllAsync(postId))
                .ReturnsAsync(reactions);

            _mapperMock
                .Setup(m => m.Map<ICollection<ResponseReactionDto>>(reactions))
                .Returns(reactionDtos);

            //Act
            var result = await _getAllPostReactionsQueryHandler.Handle(getAllPostReactionQuery, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(reactionDtos.Count, result.Count);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Never);
        }
    }
}
