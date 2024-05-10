using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.DTOs.MessageDtos
{
    public class ResponseMessageDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ConversationId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }

        //public static ResponseMessageDto FromMessage(Message message)
        //{
        //    return new ResponseMessageDto
        //    {
        //        Id = message.Id,
        //        UserId = message.UserId,
        //        ConversationId = message.ConversationId,
        //        Content = message.Content,
        //        Timestamp = message.Timestamp
        //    };
        //}
    }
}
