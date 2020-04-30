using System.Collections.Generic;
using Microsoft.Nnn.ApplicationCore.Entities.CommunityUsers;
using Microsoft.Nnn.ApplicationCore.Entities.Users;
using Microsoft.Nnn.ApplicationCore.Interfaces;

namespace Microsoft.Nnn.ApplicationCore.Entities.Communities
{
    public class Community:BaseEntity,IAggregateRoot
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoPath { get; set; }
        public virtual ICollection<CommunityUser> Users { get; set; }
    }
}