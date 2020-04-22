using System.ComponentModel;
using Microsoft.Nnn.ApplicationCore.Entities.Communities;
using Microsoft.Nnn.ApplicationCore.Entities.Users;
using Microsoft.Nnn.ApplicationCore.Interfaces;

namespace Microsoft.Nnn.ApplicationCore.Entities.CommunityUsers
{
    public class CommunityUser:BaseEntity,IAggregateRoot
    {
        public long UserId { get; set; }
        public long CommunityId { get; set; }
        
        [DefaultValue(false)]
        public bool IsAccepted { get; set; }
        
        [DefaultValue(false)]
        public bool Suspended { get; set; }
        
        public User User { get; set; }
        public Community Community { get; set; }
    }
}