namespace Nnn.ApplicationCore.Services.UserService.Dto
{
    public class ModeratorRejected
    {
        public long UserId { get; set; }
        public long CommunityId { get; set; }
        public long ModeratorId { get; set; }
    }
}