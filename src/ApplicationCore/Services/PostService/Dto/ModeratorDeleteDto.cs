namespace Microsoft.Nnn.ApplicationCore.Services.PostService.Dto
{
    public class ModeratorDeleteDto
    {
        public long PostId { get; set; }
        public long CommunityId { get; set; }
        public long ModeratorId { get; set; }
    }
}