using VisualVibes.Domain.Enum;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.DTOs.ReactionDtos
{
    public class ResponseReactionDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public ReactionType ReactionType { get; set; }
        public DateTime Timestamp { get; set; }

        //public static ResponseReactionDto FromReaction(Reaction reaction)
        //{
        //    return new ResponseReactionDto
        //    {
        //        Id = reaction.Id,
        //        UserId = reaction.UserId,
        //        PostId = reaction.PostId,
        //        ReactionType = reaction.ReactionType,
        //        Timestamp = reaction.Timestamp
        //    };
        //}
    }
}
