using System.ComponentModel;
using Microsoft.Nnn.ApplicationCore.Interfaces;

namespace Microsoft.Nnn.ApplicationCore.Entities.Notifications
{
    public class Notification:BaseEntity,IAggregateRoot
    {
        public string Content { get; set; }
        public long ContentId { get; set; }
        public string Title { get; set; }
        public long SenderId { get; set; }
        public EntityType SenderType { get; set; }
        public long OwnerId { get; set; }
        public EntityType OwnerType { get; set; }
        public NotifyContentType NotifyContentType { get; set; }
        
        [DefaultValue(false)]
        public bool IsRead { get; set; }
    }

}