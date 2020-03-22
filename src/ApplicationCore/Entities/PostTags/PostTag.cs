using Microsoft.Nnn.ApplicationCore.Entities.Posts;
using Microsoft.Nnn.ApplicationCore.Interfaces;

namespace Microsoft.Nnn.ApplicationCore.Entities.PostTags
{
    public class PostTag:BaseEntity,IAggregateRoot
    {
        public long PostId { get; set; }
        
        public long TagId { get; set; }
        
        public Post Post { get; set; }
        public Tag Tag { get; set; }
    }
}