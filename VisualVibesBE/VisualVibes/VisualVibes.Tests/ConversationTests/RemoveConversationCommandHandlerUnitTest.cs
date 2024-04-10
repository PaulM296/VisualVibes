using Moq;
using VisualVibes.App.Conversations.Commands;
using VisualVibes.App.Conversations.CommandsHandler;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Posts.Commands;
using VisualVibes.App.Posts.CommandsHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.ConversationTests
{
    public class RemoveConversationCommandHandlerUnitTest
    {
        private readonly Mock<IConversationRepository> _conversationRepositoryMock;
        private RemoveConversationCommandHandler _removeConversationCommandHandler;

        public RemoveConversationCommandHandlerUnitTest()
        {
            _conversationRepositoryMock = new Mock<IConversationRepository>();
            _removeConversationCommandHandler = new RemoveConversationCommandHandler(_conversationRepositoryMock.Object);
        }

        [Fact]
        public async void Should_RemoveConversation_Correctly()
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

            var removeConversationCommand = new RemoveConversationCommand(conversationDto.Id);

            _conversationRepositoryMock
                .Setup(x => x.GetByIdAsync(conversationDto.Id)).ReturnsAsync(conversation);

            //Act
            var result = await _removeConversationCommandHandler.Handle(removeConversationCommand, new CancellationToken());

            //Assert
            _conversationRepositoryMock.Verify(x => x.GetByIdAsync(conversationDto.Id), Times.Once);
            _conversationRepositoryMock.Verify(x => x.RemoveAsync(conversation), Times.Once);

        }
    }
}
