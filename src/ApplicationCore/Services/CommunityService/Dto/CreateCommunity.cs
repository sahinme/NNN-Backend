using System;
using Microsoft.AspNetCore.Http;

namespace Microsoft.Nnn.ApplicationCore.Services.CommunityService.Dto
{
    public class CreateCommunity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public IFormFile LogoFile { get; set; }
        public IFormFile CoverImage { get; set; }
    }
}