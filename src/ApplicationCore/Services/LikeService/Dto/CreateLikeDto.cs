using Microsoft.Nnn.ApplicationCore.Entities;

namespace Microsoft.Nnn.ApplicationCore.Services.LikeService.Dto
{
    public class CreateLikeDto
    {
        public long UserId { get; set; }
        public long EntityId { get; set; }
        public EntityType EntityType { get; set; }
    }
}