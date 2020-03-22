using Microsoft.Nnn.ApplicationCore.Entities.Posts;
using Microsoft.Nnn.ApplicationCore.Entities.Users;
using Microsoft.Nnn.ApplicationCore.Interfaces;
 
 namespace Microsoft.Nnn.ApplicationCore.Entities.Likes
 {
     public class Like:BaseEntity,IAggregateRoot
     {
         public long UserId { get; set; }
         public long PostId { get; set; }
         
         public User User { get; set; }
         public Post Post { get; set; }
     }
 }