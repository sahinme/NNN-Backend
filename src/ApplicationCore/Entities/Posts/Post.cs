using System.Collections.Generic;
using Microsoft.Nnn.ApplicationCore.Entities.Comments;
using Microsoft.Nnn.ApplicationCore.Entities.Communities;
using Microsoft.Nnn.ApplicationCore.Entities.Likes;
using Microsoft.Nnn.ApplicationCore.Entities.PostCategories;
using Microsoft.Nnn.ApplicationCore.Entities.PostTags;
using Microsoft.Nnn.ApplicationCore.Entities.Unlikes;
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
        public long CommunityId { get; set; }
        public Community Community { get; set; } 
        public User User { get; set; }
        
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Unlike> Unlikes { get; set; }
        public virtual ICollection<PostTag> Tags { get; set; }
        public virtual ICollection<PostCategory> Categories { get; set; }

    }
}