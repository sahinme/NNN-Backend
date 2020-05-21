using System;

namespace Nnn.ApplicationCore.Services.UserService.Dto
{
    public class ModeratorRejected
    {
        public Guid UserId { get; set; }
        public Guid CommunityId { get; set; }
        public Guid ModeratorId { get; set; }
    }
}