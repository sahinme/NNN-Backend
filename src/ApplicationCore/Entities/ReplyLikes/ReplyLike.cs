using Microsoft.Nnn.ApplicationCore.Entities.Replies;
using Microsoft.Nnn.ApplicationCore.Entities.Users;
using Microsoft.Nnn.ApplicationCore.Interfaces;

namespace Microsoft.Nnn.ApplicationCore.Entities.ReplyLikes
{
    public class ReplyLike:BaseEntity,IAggregateRoot
    {
        public long UserId { get; set; }
        public long ReplyId { get; set; }

        public User User { get; set; }
        public Reply Reply { get; set; }
    }
}