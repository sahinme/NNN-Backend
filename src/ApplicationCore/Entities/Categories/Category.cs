using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Nnn.ApplicationCore.Interfaces;

namespace Microsoft.Nnn.ApplicationCore.Entities.Categories
{
    public class Category:BaseEntity,IAggregateRoot
    {
        public string DisplayName { get; set; }

        public string Description { get; set; }
        
        public long? ParentCategoryId { get; set; }
        
        
        [ForeignKey(nameof(ParentCategoryId))]
        public virtual Category ParentCategory { get; set; }
    }
}