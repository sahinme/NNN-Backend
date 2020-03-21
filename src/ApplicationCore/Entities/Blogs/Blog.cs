using Microsoft.Nnn.ApplicationCore.Interfaces;

namespace Microsoft.Nnn.ApplicationCore.Entities.Blogs
{
    public class Blog:BaseEntity,IAggregateRoot
    {
        public string Content { get; set; }
    }
}