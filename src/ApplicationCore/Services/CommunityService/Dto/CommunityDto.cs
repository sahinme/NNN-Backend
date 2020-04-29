using System.Collections.Generic;
using Nnn.ApplicationCore.Services.UserService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.CommunityService.Dto
{
    public class CommunityDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoPath { get; set; }
       public List<CommunityUserDto> Members { get; set; }
    }

    public class CommunityUserDto
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string ProfileImg { get; set; }
    }
}