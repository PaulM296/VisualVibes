using Moq;
using VisualVibes.App.Conversations.Commands;
using VisualVibes.App.Conversations.CommandsHandler;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.ConversationTests
{
    public class CreateConversationCommandHandlerUnitTest
    {
        private readonly Mock<IConversationRepository> _conversationRepositoryMock;
        private CreateConversationCommandHandler _createConversationCommandHandler;

        public CreateConversationCommandHandlerUnitTest()
        {
            _conversationRepositoryMock = new Mock<IConversationRepository>();
            _createConversationCommandHandler = new CreateConversationCommandHandler(_conversationRepositoryMock.Object);
        }

        [Fact]
        public async void Should_CreateConverstion_Correclty()
        {
            //Arrange
            var conversationDto = new ConversationDto
            {
                Id = Guid.NewGuid(),
                FirstParticipantId = Guid.NewGuid(),
                SecondParticipantId = Guid.NewGuid(),
            };

            var conversation = new Conversation
            {
                Id = conversationDto.Id,
                FirstParticipantId = conversationDto.FirstParticipantId,
                SecondParticipantId = conversationDto.SecondParticipantId,
            };

            var createConversationCommand = new CreateConversationCommand(conversationDto);

            _conversationRepositoryMock
                .Setup(x => x.AddAsync(It.Is<Conversation>(y => y.FirstParticipantId == conversationDto.FirstParticipantId && 
                y.SecondParticipantId == conversationDto.SecondParticipantId))).ReturnsAsync(conversation);

            //Act
            var result = await _createConversationCommandHandler.Handle(createConversationCommand, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(conversation.FirstParticipantId, result.FirstParticipantId);
            Assert.Equal(conversation.SecondParticipantId, result.SecondParticipantId);
        }
    }
}
