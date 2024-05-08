using Moq;
using VisualVibes.App.Conversations.Queries;
using VisualVibes.App.Conversations.QueriesHandlers;
using VisualVibes.App.DTOs.ConversationDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Queries;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.ConversationTests
{
    public class GetAllUserConversationsQueryHandlerUnitTest
    {
        private readonly Mock<IConversationRepository> _conversationRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private GetAllUserConversationsQueryHandler _getAllUserConversationsQueryHandler;

        public GetAllUserConversationsQueryHandlerUnitTest()
        {
            _conversationRepositoryMock = new Mock<IConversationRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _unitOfWorkMock.Setup(uow => uow.ConversationRepository).Returns(_conversationRepositoryMock.Object);
            _getAllUserConversationsQueryHandler = new GetAllUserConversationsQueryHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async void Should_GetUserById_Correctly()
        {
            //Arrange
            var userId = Guid.NewGuid();

            var conversationDtos = new List<ResponseConversationDto>
            {
                new ResponseConversationDto
                {
                    Id = Guid.NewGuid(),
                    FirstParticipantId = userId,
                    SecondParticipantId = Guid.NewGuid(),
                },
                new ResponseConversationDto
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
                .Setup(x => x.GetAllByUserIdAsync(userId))
                .ReturnsAsync(conversations);

            //Act
            var result = await _getAllUserConversationsQueryHandler.Handle(getAllUserConversationsQuery, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(conversationDtos.Count, result.Count);
            foreach (var conversation in result)
            {
                Assert.Contains(conversation.Id, conversations.Select(c => c.Id));
            }
        }
    }
}
