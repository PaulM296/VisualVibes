using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using VisualVibes.App.Conversations.Queries;
using VisualVibes.App.Conversations.QueriesHandlers;
using VisualVibes.App.DTOs.ConversationDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Messages.CommandsHandler;
using VisualVibes.App.Users.Queries;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.ConversationTests
{
    public class GetAllUserConversationsQueryHandlerUnitTest
    {
        private readonly Mock<IConversationRepository> _conversationRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<GetAllUserConversationsQueryHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllUserConversationsQueryHandler _getAllUserConversationsQueryHandler;

        public GetAllUserConversationsQueryHandlerUnitTest()
        {
            _conversationRepositoryMock = new Mock<IConversationRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<GetAllUserConversationsQueryHandler>>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(uow => uow.ConversationRepository).Returns(_conversationRepositoryMock.Object);
            
            _getAllUserConversationsQueryHandler = new GetAllUserConversationsQueryHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async void Should_GetUserById_Correctly()
        {
            //Arrange
            var userId = Guid.NewGuid();

            var conversations = new List<Conversation>
            {
                new Conversation 
                { 
                    Id = Guid.NewGuid(), 
                    FirstParticipantId = userId, 
                    SecondParticipantId = Guid.NewGuid() 
                },

                new Conversation 
                { 
                    Id = Guid.NewGuid(), 
                    FirstParticipantId = Guid.NewGuid(), 
                    SecondParticipantId = userId 
                }
            };

            var conversationDtos = conversations.Select(c => new ResponseConversationDto
            {
                Id = c.Id,
                FirstParticipantId = c.FirstParticipantId,
                SecondParticipantId = c.SecondParticipantId
            }).ToList();

            var getAllUserConversationsQuery = new GetAllUserConversationsQuery(userId);

            _conversationRepositoryMock
                .Setup(x => x.GetAllByUserIdAsync(userId))
                .ReturnsAsync(conversations);

            _mapperMock
                .Setup(m => m.Map<ICollection<ResponseConversationDto>>(conversations))
                .Returns(conversationDtos);


            //Act
            var result = await _getAllUserConversationsQueryHandler.Handle(getAllUserConversationsQuery, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(conversationDtos.Count, result.Count);
            _mapperMock.Verify(m => m.Map<ICollection<ResponseConversationDto>>(conversations), Times.Once);
        }

        [Fact]
        public async Task Should_ThrowWhenNoConversations_Found()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var query = new GetAllUserConversationsQuery(userId);

            _conversationRepositoryMock.Setup(x => x.GetAllByUserIdAsync(userId)).ReturnsAsync(new List<Conversation>());

            // Act & Assert
            await Assert.ThrowsAsync<ConversationNotFoundException>(() => _getAllUserConversationsQueryHandler.Handle(query, new CancellationToken()));
        }
    }
}
