using Microsoft.Nnn.ApplicationCore.Interfaces;

namespace Microsoft.Nnn.ApplicationCore.Entities.Moderators
{
    public class Moderator:BaseEntity,IAggregateRoot
    {
        public long UserId { get; set; }
        public long CommunityId { get; set; }
    }
}