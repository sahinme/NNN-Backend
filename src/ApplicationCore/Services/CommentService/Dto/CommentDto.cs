using System;
using System.Collections.Generic;
using Microsoft.Nnn.ApplicationCore.Services.ReplyService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.CommentService.Dto
{
    public class CommentDto
    {
        public long Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string Content { get; set; }
        public long LikeCount { get; set; }
        public CommentUserDto CommentUserInfo { get; set; }
        public List<ReplyDto> Replies { get; set; }
    }

    public class CommentUserDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string ProfileImagePath { get; set; }
    }
}