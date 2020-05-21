using System;

namespace Microsoft.Nnn.ApplicationCore.Services.PostService.Dto
{
    public class ModeratorDeleteDto
    {
        public Guid PostId { get; set; }
        public Guid CommunityId { get; set; }
        public Guid ModeratorId { get; set; }
    }
}