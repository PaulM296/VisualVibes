using Moq;
using VisualVibes.App.Conversations.Queries;
using VisualVibes.App.Conversations.QueriesHandlers;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Queries;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.ConversationTests
{
    public class GetAllUserConversationsQueryHandlerUnitTest
    {
        private readonly Mock<IConversationRepository> _conversationRepositoryMock;
        private GetAllUserConversationsQueryHandler _getAllUserConversationsQueryHandler;

        public GetAllUserConversationsQueryHandlerUnitTest()
        {
            _conversationRepositoryMock = new Mock<IConversationRepository>();
            _getAllUserConversationsQueryHandler = new GetAllUserConversationsQueryHandler(_conversationRepositoryMock.Object);
        }

        [Fact]
        public async void Should_GetUserById_Correctly()
        {
            //Arrange
            var userId = Guid.NewGuid();

            var conversationDtos = new List<ConversationDto>
            {
                new ConversationDto
                {
                    Id = Guid.NewGuid(),
                    FirstParticipantId = userId,
                    SecondParticipantId = Guid.NewGuid(),
                },
                new ConversationDto
                {
                    Id = Guid.NewGuid(),
                    FirstParticipantId = Guid.NewGuid(),
                    SecondParticipantId = userId,
                }
            };

            var conversations = new List<Conversation>();
            foreach(var conversationDto in conversationDtos)
            {
                conversations.Add( new Conversation
                {
                    Id = conversationDto.Id,
                    FirstParticipantId = conversationDto.FirstParticipantId,
                    SecondParticipantId = conversationDto.SecondParticipantId
                });
            }

            var getAllUserConversationsQuery = new GetAllUserConversationsQuery(userId);

            _conversationRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(conversations);

            //Act
            var result = await _getAllUserConversationsQueryHandler.Handle(getAllUserConversationsQuery, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(conversationDtos.Count, result.Count);
        }
    }
}
