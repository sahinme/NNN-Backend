using Microsoft.Nnn.ApplicationCore.Entities.Comments;
using Microsoft.Nnn.ApplicationCore.Entities.Users;
using Microsoft.Nnn.ApplicationCore.Interfaces;

namespace Microsoft.Nnn.ApplicationCore.Entities.Replies
{
    public class Reply:BaseEntity,IAggregateRoot
    {
        public string Content { get; set; }
        public long UserId { get; set; }
        public long CommentId { get; set; }
        
        public User User { get; set; }
        public Comment Comment { get; set; }
    }
}