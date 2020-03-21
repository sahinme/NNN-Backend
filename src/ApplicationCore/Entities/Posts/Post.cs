using System.Collections.Generic;
using Microsoft.Nnn.ApplicationCore.Entities.Comments;
using Microsoft.Nnn.ApplicationCore.Entities.Users;
using Microsoft.Nnn.ApplicationCore.Interfaces;

namespace Microsoft.Nnn.ApplicationCore.Entities.Posts
{
    public class Post:BaseEntity,IAggregateRoot
    {
        public string Title { get; set; }
        public string Why { get; set; }
        public string How { get; set; }
        public string Where { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

    }
}