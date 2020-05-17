using System.ComponentModel;
using Microsoft.Nnn.ApplicationCore.Interfaces;

namespace Microsoft.Nnn.ApplicationCore.Entities.Notifications
{
    public class Notification:BaseEntity,IAggregateRoot
    {
        public string Content { get; set; }
        public NotifyContentType Type { get; set; }
        public long TargetId { get; set; }
        public long OwnerUserId { get; set; }
    }

}