using System;
using Microsoft.Nnn.ApplicationCore.Services.CategoryService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.CommunityService.Dto
{
    public class GetAllCommunityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoPath { get; set; }
        public int MemberCount { get; set; }
        public bool IsUserJoined { get; set; }
        public CategoryDto Category { get; set; }
    }
}