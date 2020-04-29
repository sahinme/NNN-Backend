namespace Microsoft.Nnn.ApplicationCore.Services.CommunityService.Dto
{
    public class GetAllCommunityDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoPath { get; set; }
        public int MemberCount { get; set; }
    }
}