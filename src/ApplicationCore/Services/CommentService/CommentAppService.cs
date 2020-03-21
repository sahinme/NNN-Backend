using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Nnn.ApplicationCore.Entities.Comments;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Microsoft.Nnn.ApplicationCore.Services.CommentService.Dto;
using Microsoft.Nnn.ApplicationCore.Services.ReplyService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.CommentService
{
    public class CommentAppService:ICommentAppService
    {
        private readonly IAsyncRepository<Comment> _commentRepository;

        public CommentAppService(IAsyncRepository<Comment> commentRepository)
        {
            _commentRepository = commentRepository;
        }
        
        public async Task<Comment> CreateComment(CreateCommentDto input)
        {
            var comment = new Comment
            {
                Content = input.Content,
                UserId = input.UserId,
                PostId = input.PostId
            };
           await _commentRepository.AddAsync(comment);
           return comment;
        }

        public async Task<List<CommentDto>> GetPostComments(long postId)
        {
            var postComments = await _commentRepository.GetAll().Where(x => x.IsDeleted == false && x.PostId == postId)
                .Include(x => x.Post).Include(x => x.User)
                .Include(x=>x.Replies).ThenInclude(x=>x.User)
                .Select(x => new CommentDto
                {
                    Id = x.Id,
                    Content = x.Content,
                    CreatedDateTime = x.CreatedDate,
                    CommentUserInfo = new CommentUserDto
                    {
                        Id = x.User.Id,
                        UserName = x.User.Username,
                        ProfileImagePath = x.User.ProfileImagePath
                    },
                    Replies = x.Replies.Where(r=>r.IsDeleted==false).Select(r => new ReplyDto
                    {
                        Id = r.Id,
                        Content = r.Content,
                        CreatedDateTime = r.CreatedDate,
                        ReplyUserInfo = new ReplyUserDto
                        {
                            Id = r.User.Id,
                            UserName = r.User.Username,
                            ProfileImagePath = r.User.ProfileImagePath
                        }
                    }).ToList()
                }).ToListAsync();
            return postComments;
        }
    }
}