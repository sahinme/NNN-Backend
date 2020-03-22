using System;
using System.Collections.Generic;
using Microsoft.Nnn.ApplicationCore.Services.CategoryService.Dto;
using Microsoft.Nnn.ApplicationCore.Services.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.PostAppService.Dto
{
    public class PostDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Why { get; set; }
        public string How { get; set; }
        public string Where { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public PostUserDto UserInfo { get; set; }
        public List<CategoryDto> Categories { get; set; }
        public List<TagDto> Tags { get; set; }
    }

    public class PostUserDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string ProfileImagePath { get; set; }
    }
}