using Microsoft.AspNetCore.Http;

namespace Microsoft.Nnn.ApplicationCore.Services.CommunityService.Dto
{
    public class UpdateCommunity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public IFormFile Logo { get; set; }
        public IFormFile CoverPhoto { get; set; }
        public string Description { get; set; }
        public long ModeratorId { get; set; }
    }
}