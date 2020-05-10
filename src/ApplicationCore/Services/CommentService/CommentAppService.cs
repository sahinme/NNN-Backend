using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Nnn.ApplicationCore.Entities.CommentLikes;
using Microsoft.Nnn.ApplicationCore.Entities.Comments;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Microsoft.Nnn.ApplicationCore.Services.CommentService.Dto;
using Microsoft.Nnn.ApplicationCore.Services.ReplyService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.CommentService
{
    public class CommentAppService:ICommentAppService
    {
        private readonly IAsyncRepository<Comment> _commentRepository;
        private readonly IAsyncRepository<CommentLike> _commentLikeRepository;

        public CommentAppService(IAsyncRepository<Comment> commentRepository,IAsyncRepository<CommentLike> commentLikeRepository)
        {
            _commentRepository = commentRepository;
            _commentLikeRepository = commentLikeRepository;
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

        public async Task<Comment> UpdateComment(UpdateComment input)
        {
            var comment = await _commentRepository.GetByIdAsync(input.Id);
            comment.Content = input.Content;
            await _commentRepository.UpdateAsync(comment);
            return comment;
        }

        public async Task Delete(long id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            comment.IsDeleted = true;
            await _commentRepository.UpdateAsync(comment);
        }

        public async Task<CommentLike> Like(long userId, long commentId)
        {
            var isExist = await _commentLikeRepository.GetAll()
                .FirstOrDefaultAsync(x => x.IsDeleted==false && x.UserId == userId && x.CommentId == commentId);
            
            if (isExist != null)
            { 
                throw  new Exception("Bu islem daha once yapilmis");
            }

            var model = new CommentLike
            {
                UserId = userId,
                CommentId = commentId
            };

            await _commentLikeRepository.AddAsync(model);
            return model;
        }
        
        public async Task Unlike(long userId, long commentId)
        {
            var isExist = await _commentLikeRepository.GetAll()
                .FirstOrDefaultAsync(x => x.IsDeleted==false && x.UserId == userId && x.CommentId == commentId);
            
            if (isExist == null)
            {
                throw  new Exception("Boyle bir islem yok");
            }

            isExist.IsDeleted = true;
            await _commentLikeRepository.UpdateAsync(isExist);

        }
    }
}