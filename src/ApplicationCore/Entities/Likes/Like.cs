using Microsoft.Nnn.ApplicationCore.Entities.Comments;
using Microsoft.Nnn.ApplicationCore.Entities.Posts;
using Microsoft.Nnn.ApplicationCore.Entities.Replies;
using Microsoft.Nnn.ApplicationCore.Entities.Users;
using Microsoft.Nnn.ApplicationCore.Interfaces;
 
 namespace Microsoft.Nnn.ApplicationCore.Entities.Likes
 {
     public class Like:BaseEntity,IAggregateRoot
     {
         public long UserId { get; set; }
         public long EntityId { get; set; }
         public EntityType EntityType { get; set; }
         
         public User User { get; set; }
         public Post Post { get; set; }
         public Comment Comment { get; set; } 
         public Reply Reply { get; set; }
     }
 }