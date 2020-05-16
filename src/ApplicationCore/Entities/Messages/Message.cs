using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Nnn.ApplicationCore.Entities.Conversations;
using Microsoft.Nnn.ApplicationCore.Entities.Users;
using Microsoft.Nnn.ApplicationCore.Interfaces;

namespace Microsoft.Nnn.ApplicationCore.Entities.Messages
{
    public class Message:BaseEntity,IAggregateRoot
    {
        public long SenderId { get; set; }
        public long ReceiverId { get; set; }
        public long ConversationId { get; set; }
        public string Content { get; set; }
        
        [DefaultValue(false)]
        public bool IsRead { get; set; }
        
        [ForeignKey(nameof(SenderId))]
        public virtual User Sender { get; set; }
        
        [ForeignKey(nameof(ReceiverId))]
        public virtual User Receiver { get; set; }

        public Conversation Conversation { get; set; }
    }
}