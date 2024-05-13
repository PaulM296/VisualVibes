using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using VisualVibes.App.Conversations.Commands;
using VisualVibes.App.Conversations.CommandsHandler;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Messages.CommandsHandler;
using VisualVibes.App.Posts.Commands;
using VisualVibes.App.Posts.CommandsHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.ConversationTests
{
    public class RemoveConversationCommandHandlerUnitTest
    {
        private readonly Mock<IConversationRepository> _conversationRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<RemoveConversationCommandHandler>> _loggerMock;
        private readonly RemoveConversationCommandHandler _removeConversationCommandHandler;

        public RemoveConversationCommandHandlerUnitTest()
        {
            _conversationRepositoryMock = new Mock<IConversationRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<RemoveConversationCommandHandler>>();

            _unitOfWorkMock.Setup(uow => uow.ConversationRepository).Returns(_conversationRepositoryMock.Object);
            
            _removeConversationCommandHandler = new RemoveConversationCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async void Should_RemoveConversation_Correctly()
        {
            //Arrange
            var conversationId = Guid.NewGuid();

            var conversation = new Conversation
            {
                Id = conversationId,
                FirstParticipantId = Guid.NewGuid(),
                SecondParticipantId = Guid.NewGuid(),
            };


            var removeConversationCommand = new RemoveConversationCommand(conversationId);

            _conversationRepositoryMock
                .Setup(x => x.GetByIdAsync(conversationId))
                .ReturnsAsync(conversation);

            //Act
            var result = await _removeConversationCommandHandler.Handle(removeConversationCommand, new CancellationToken());

            //Assert
            _conversationRepositoryMock.Verify(x => x.GetByIdAsync(conversationId), Times.Once);
            _conversationRepositoryMock.Verify(x => x.RemoveAsync(conversation), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);

        }
    }
}
