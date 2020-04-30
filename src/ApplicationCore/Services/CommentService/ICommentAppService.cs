using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Nnn.ApplicationCore.Entities.CommentLikes;
using Microsoft.Nnn.ApplicationCore.Entities.Comments;
using Microsoft.Nnn.ApplicationCore.Services.CommentService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.CommentService
{
    public interface ICommentAppService
    {
        Task<Comment> CreateComment(CreateCommentDto input);
        Task<List<CommentDto>> GetPostComments(long postId);
        Task<Comment> UpdateComment(UpdateComment input);
        Task Delete(long id);
        Task<CommentLike> Like(long userId,long commentId);
        Task Unlike(long userId, long commentId);
    }
}