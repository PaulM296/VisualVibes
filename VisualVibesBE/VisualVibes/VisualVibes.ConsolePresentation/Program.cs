using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.Common;
using VisualVibes.App;
using VisualVibes.App.Comments.Commands;
using VisualVibes.App.Comments.Queries;
using VisualVibes.App.Conversations.Commands;
using VisualVibes.App.Conversations.Queries;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Messages.Commands;
using VisualVibes.App.Messages.Queries;
using VisualVibes.App.Posts.Commands;
using VisualVibes.App.Posts.Queries;
using VisualVibes.App.Reactions.Commands;
using VisualVibes.App.Reactions.Queries;
using VisualVibes.App.UserProfiles.Commands;
using VisualVibes.App.Users.Commands;
using VisualVibes.App.Users.Queries;
using VisualVibes.App.Users.QueriesHandler;
using VisualVibes.Domain.Enum;
using VisualVibes.Domain.Models.BaseEntity;
using VisualVibes.Infrastructure.Repositories;


var diContainer = new ServiceCollection()
                 .AddScoped<IUserRepository, UserRepository>()
                 .AddScoped<IPostRepository, PostRepository>()
                 .AddScoped<ICommentRepository, CommentRepository>()
                 .AddScoped<IReactionRepository, ReactionRepository>()
                 .AddScoped<IUserProfileRepository, UserProfileRepository>()
                 .AddScoped<IFeedRepository, FeedRepository>()
                 .AddScoped<IMessageRepository, MessageRepository>()
                 .AddScoped<IConversationRepository, ConversationRepository>()
                 .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IUserRepository).Assembly))
                 .AddTransient(provider => new FileSystemLogger("logs"))
                 .BuildServiceProvider();

var logger = diContainer.GetRequiredService<FileSystemLogger>();

var mediator = diContainer.GetRequiredService<IMediator>();

Console.WriteLine("Testing UserDto.\n");

var userDto = new UserDto
{
    Id = Guid.NewGuid(),
    Username = "Paulinho",
    Password = "123456789",
    Followers = new List<UserDto>(),
    Following = new List<UserDto>()
};

var userDto2 = new UserDto
{
    Id = Guid.NewGuid(),
    Username = "Waganaha",
    Password = "456123",
    Followers = new List<UserDto>(),
    Following = new List<UserDto>()
};

var createdUser = await mediator.Send(new CreateUserCommand(userDto));
Console.WriteLine($"User ID: {createdUser.Id}, Username: {createdUser.Username} , Password: {createdUser.Password}");

var retrievedUser = await mediator.Send(new GetUserByIdQuery(createdUser.Id));
Console.WriteLine($"Retrieved user successfully! ID: {retrievedUser.Id}");

try
{
    var updatedUserDto = new UserDto
    {
        Id = createdUser.Id,
        Username = "UpdatedUsername",
        Password = "UpdatedPassword",
        Followers = new List<UserDto>(),
        Following = new List<UserDto>()
    };
    var updatedUser = await mediator.Send(new UpdateUserCommand(updatedUserDto));
    Console.WriteLine($"User ID: {createdUser.Id}, Username: {updatedUser.Username} , Password: {updatedUser.Password}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

Console.WriteLine("\n\nTesting UserProfileDto.\n");

var userProfileDto = new UserProfileDto
{
    Id = Guid.NewGuid(),
    ProfilePicture = "profilePicture.jpg",
    DateOfBirth = new DateTime(2000, 06, 29),
    FirstName = "Paul",
    LastName = "Micluta",
    Email = "paulmicluta@gmail.com",
    Bio = "This is a bio."
};

var createdProfile = await mediator.Send(new CreateUserProfileCommand(userProfileDto));
Console.WriteLine($"Profile created successfully! ID: {createdProfile.Id}, Bio: {createdProfile.Bio}");

var updatedUserProfile = new UserProfileDto
{
    Id = createdProfile.Id,
    ProfilePicture = "profilePicture.jpg",
    DateOfBirth = new DateTime(2000, 06, 29),
    FirstName = "Paul",
    LastName = "Micluta",
    Email = "paulmicluta@gmail.com",
    Bio = "This is an updated bio."
};

var updatedProfile = await mediator.Send(new UpdateUserProfileCommand(updatedUserProfile));
Console.WriteLine($"Profile updated successfully! ID: {updatedProfile.Id}, Bio: {updatedProfile.Bio}");

Console.WriteLine("\n\nTesting PostDto.\n");

var postDto = new PostDto
{
    Id = Guid.NewGuid(),
    UserId = createdUser.Id,
    Caption = "This is a new post",
    Pictures = "picture1",
    CreatedAt = DateTime.UtcNow
};

var createdPost = await mediator.Send(new CreatePostCommand(postDto));

Console.WriteLine($"Created post: ID: {createdPost.Id}, From UserId: {postDto.UserId}, Caption: {createdPost.Caption}");

var retrievedPost = await mediator.Send(new GetPostByIdQuery(createdPost.Id));
Console.WriteLine($"Retrieved post successfully! ID: {retrievedPost.Id}");

var updatedPostDto = new PostDto
{
    Id = createdPost.Id,
    UserId = createdPost.UserId,
    Caption = "This is a new post",
    Pictures = "picture 1 picture2",
    CreatedAt = DateTime.UtcNow
};

var updatedPost = await mediator.Send(new UpdatePostCommand(updatedPostDto));
Console.WriteLine($"Updated post: Caption: {updatedPost.Caption}, Pictures: {updatedPost.Pictures}");

Console.WriteLine("\n\nTesting ConversationDto.\n");

var conversationDto = new ConversationDto
{
    Id = Guid.NewGuid(),
    FirstParticipantId = userDto.Id,
    SecondParticipantId = userDto2.Id
};

var createdConversation = await mediator.Send(new CreateConversationCommand(conversationDto));
Console.WriteLine($"User ID: {createdConversation.Id}, FirstParticipant: {createdConversation.FirstParticipantId}," +
    $" SecondParticipant: {createdConversation.SecondParticipantId}");

var conversations = await mediator.Send(new GetAllUserConversationsQuery(createdConversation.FirstParticipantId));
foreach (var conversation in conversations)
{
    Console.WriteLine($"Conversation ID: {conversation.Id}, First Participant: {conversation.FirstParticipantId}, Second Participant: {conversation.SecondParticipantId}");
}

Console.WriteLine("\n\nTesting MessageDto.\n");

var messageDto = new MessageDto
{
    Id = Guid.NewGuid(),
    SenderId = userDto.Id,
    ReceiverId = userDto2.Id,
    ConversationId = conversationDto.Id,
    Content = "Hello, how are you?",
    Timestamp = DateTime.UtcNow
};

var messageDto2 = new MessageDto
{
    Id = Guid.NewGuid(),
    SenderId = userDto2.Id,
    ReceiverId = userDto.Id,
    ConversationId = conversationDto.Id,
    Content = "I'm doing well, thank you! What about you?",
    Timestamp = DateTime.UtcNow.AddHours(0.5)
};

var createdMessage1 = await mediator.Send(new CreateMessageCommand(messageDto));
Console.WriteLine($"Created message: ID: {createdMessage1.Id}, Sender: {createdMessage1.SenderId}," +
    $" Receiver: {createdMessage1.ReceiverId}, Content: {createdMessage1.Content}");

var createdMessage2 = await mediator.Send(new CreateMessageCommand(messageDto2));
Console.WriteLine($"Created message: ID: {createdMessage2.Id}, Sender: {createdMessage2.SenderId}, " +
    $"Receiver: {createdMessage2.ReceiverId}, Content: {createdMessage2.Content}");

var getAllConversationMessagesQuery = new GetAllConversationMessagesQuery(conversationDto.Id);
var conversationMessages = await mediator.Send(getAllConversationMessagesQuery);

foreach (var message in conversationMessages)
{
    Console.WriteLine($"Message ID: {message.Id}, Sender: {message.SenderId}, Receiver: {message.ReceiverId}, Content: {message.Content}");
}

Console.WriteLine("\n\nTesting CommentDto.\n");

var commentDto = new CommentDto
{
    Id = Guid.NewGuid(),
    UserId = userDto.Id,
    PostId = postDto.Id,
    Text = "This is a sample comment.",
    CreatedAt = DateTime.UtcNow
};

var createdCommentDto = await mediator.Send(new CreateCommentCommand(commentDto));

Console.WriteLine($"Created comment: ID: {createdCommentDto.Id}, User ID: {createdCommentDto.UserId}," +
    $" Post ID: {createdCommentDto.PostId}, Text: {createdCommentDto.Text}, Created At: {createdCommentDto.CreatedAt}");

var commentDto2 = new CommentDto
{
    Id = Guid.NewGuid(),
    UserId = userDto.Id,
    PostId = postDto.Id,
    Text = "This is another sample comment.",
    CreatedAt = DateTime.UtcNow
};

var createdCommentDto2 = await mediator.Send(new CreateCommentCommand(commentDto2));

Console.WriteLine($"Created comment: ID: {createdCommentDto2.Id}, User ID: {createdCommentDto2.UserId}," +
    $" Post ID: {createdCommentDto2.PostId}, Text: {createdCommentDto2.Text}, Created At: {createdCommentDto2.CreatedAt}");

var updatedCommentDto = new CommentDto
{
    Id = createdCommentDto.Id,
    UserId = createdCommentDto.UserId,
    PostId = createdCommentDto.PostId,
    Text = "This is an updated sample comment.",
    CreatedAt = DateTime.UtcNow
};

var updatedComment = await mediator.Send(new UpdateCommentCommand(updatedCommentDto));
Console.WriteLine($"Updated comment: ID: {updatedComment.Id}, User ID: {updatedComment.UserId}," +
    $" Post ID: {updatedComment.PostId}, Text: {updatedComment.Text}, Created At: {updatedComment.CreatedAt}");

var getAllPostComments = new GetAllPostCommentsCommand(postDto.Id);
var postComments = await mediator.Send(getAllPostComments);

foreach (var comment in postComments)
{
    Console.WriteLine($"Comment ID: {comment.Id}, UserId: {comment.UserId}, PostId: {comment.PostId}, Text: {comment.Text}");
}

Console.WriteLine("\n\nTesting ReactionDto.\n");

var reactionDto1 = new ReactionDto
{
    Id = Guid.NewGuid(),
    UserId = userDto.Id,
    PostId = postDto.Id,
    ReactionType = ReactionType.Like,
    Timestamp = DateTime.UtcNow
};

var createdReactionCommand1 = await mediator.Send(new CreateReactionCommand(reactionDto1));
Console.WriteLine($"Reaction Id: {createdReactionCommand1.Id}, User ID: {createdReactionCommand1.UserId}," +
    $" Post ID: {createdReactionCommand1.PostId}, ReactionType: {createdReactionCommand1.ReactionType}");

var reactionDto2 = new ReactionDto
{
    Id = Guid.NewGuid(),
    UserId = userDto2.Id,
    PostId = postDto.Id,
    ReactionType = ReactionType.Love,
    Timestamp = DateTime.UtcNow
};

var createdReactionCommand2 = await mediator.Send(new CreateReactionCommand(reactionDto2));
Console.WriteLine($"Reaction ID: {createdReactionCommand2.Id}, User ID: {createdReactionCommand2.UserId}," +
    $" Post ID: {createdReactionCommand2.PostId}, ReactionType: {createdReactionCommand2.ReactionType}");

var getAllPostReactions = new GetAllPostReactionsCommand(postDto.Id);
var postReactions = await mediator.Send(getAllPostReactions);

foreach (var reaction in postReactions)
{
    Console.WriteLine($"Comment ID: {reaction.Id}, UserId: {reaction.UserId}, PostId: {reaction.PostId}, ReactionType: {reaction.ReactionType}");
}

Console.WriteLine("\n\nTesting RemoveUserDto.\n");

try
{
    await mediator.Send(new RemoveUserCommand(userDto.Id));
    Console.WriteLine($"User deleted successfully");
}
catch (Exception ex)
{
    Console.WriteLine($"Error removing user: {ex.Message}");
}

Console.WriteLine("\n\nTesting RemovePostDto.\n");

try
{
    await mediator.Send(new RemovePostCommand(createdPost.Id));
    Console.WriteLine($"Post deleted successfully");
}
catch (Exception ex)
{
    Console.WriteLine($"Error removing post: {ex.Message}");
}

Console.WriteLine("\n\nTesting RemoveConversationDto.\n");

try
{
    await mediator.Send(new RemoveConversationCommand(createdConversation.Id));
    Console.WriteLine($"Conversation deleted successfully");
}
catch (Exception ex)
{
    Console.WriteLine($"Error removing conversation: {ex.Message}");
}

Console.WriteLine("\n\nTesting RemoveMessage.\n");

try
{
    await mediator.Send(new RemoveMessagedCommand(createdMessage1.Id));
    Console.WriteLine($"Message deleted successfully");
}
catch (Exception ex)
{
    Console.WriteLine($"Error removing message: {ex.Message}");
}

Console.WriteLine("\n\nTesting RemoveComment.\n");

try
{
    await mediator.Send(new RemoveCommentCommand(commentDto.Id));
    Console.WriteLine($"Comment deleted successfully");
}
catch (Exception ex)
{
    Console.WriteLine($"Error removing comment: {ex.Message}");
}

Console.WriteLine("\n\nTesting RemoveReaction.\n");

try
{
    await mediator.Send(new RemoveReactionCommand(reactionDto1.Id));
    Console.WriteLine($"Reaction deleted successfully");
}
catch (Exception ex)
{
    Console.WriteLine($"Error removing reaction: {ex.Message}");
}