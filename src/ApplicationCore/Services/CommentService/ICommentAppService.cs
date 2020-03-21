using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Nnn.ApplicationCore.Entities.Comments;
using Microsoft.Nnn.ApplicationCore.Services.CommentService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.CommentService
{
    public interface ICommentAppService
    {
        Task<Comment> CreateComment(CreateCommentDto input);
        Task<List<CommentDto>> GetPostComments(long postId);
    }
}