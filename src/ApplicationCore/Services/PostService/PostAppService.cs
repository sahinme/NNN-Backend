using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Nnn.ApplicationCore.Entities.Comments;
using Microsoft.Nnn.ApplicationCore.Entities.Communities;
using Microsoft.Nnn.ApplicationCore.Entities.CommunityUsers;
using Microsoft.Nnn.ApplicationCore.Entities.ModeratorOperations;
using Microsoft.Nnn.ApplicationCore.Entities.Notifications;
using Microsoft.Nnn.ApplicationCore.Entities.PostCategories;
using Microsoft.Nnn.ApplicationCore.Entities.Posts;
using Microsoft.Nnn.ApplicationCore.Entities.PostTags;
using Microsoft.Nnn.ApplicationCore.Entities.PostVotes;
using Microsoft.Nnn.ApplicationCore.Entities.Users;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Microsoft.Nnn.ApplicationCore.Services.BlobService;
using Microsoft.Nnn.ApplicationCore.Services.CommentService.Dto;
using Microsoft.Nnn.ApplicationCore.Services.Dto;
using Microsoft.Nnn.ApplicationCore.Services.PostAppService.Dto;
using Microsoft.Nnn.ApplicationCore.Services.PostService.Dto;
using Microsoft.Nnn.ApplicationCore.Services.ReplyService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.PostService
{
    public class PostAppService:IPostAppService
    {
        private readonly IAsyncRepository<Post> _postRepository;
        private readonly IAsyncRepository<PostVote> _postVoteRepository;
        private readonly IAsyncRepository<CommunityUser> _communityUserRepository;
        private readonly IAsyncRepository<ModeratorOperation> _moderatorOperationRepository;
        private readonly IAsyncRepository<User> _userRepository;
        private readonly IAsyncRepository<Notification> _notificationRepository;
        private readonly IBlobService _blobService;
        private readonly IAsyncRepository<Community> _communityRepository;

        public PostAppService(IAsyncRepository<Post> postRepository, 
            IBlobService blobService,
           IAsyncRepository<CommunityUser> communityUserRepository,
            IAsyncRepository<Community> communityRepository,
            IAsyncRepository<PostVote> postVoteRepository,
            IAsyncRepository<ModeratorOperation> moderatorOperationRepository,
            IAsyncRepository<Notification> notificationRepository,
            IAsyncRepository<User> userRepository)
        {
            _postRepository = postRepository;
            _blobService = blobService;
            _communityUserRepository = communityUserRepository;
            _postVoteRepository = postVoteRepository;
            _moderatorOperationRepository = moderatorOperationRepository;
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _communityRepository = communityRepository;
        }
        
        public async Task<Post> CreatePost(CreatePostDto input)
        {
            var post = new Post
            {
               Content = input.Content,
               CommunityId = input.CommunityId,
               UserId = input.UserId,
               ContentType = input.ContentType
            };
            
            if (input.ContentFile != null)
            {
                var path = await _blobService.InsertFile(input.ContentFile);
                post.MediaContentPath = path;
            }

            if (input.ContentType == ContentType.Link)
            {
                post.LinkUrl = input.LinkUrl;
            }
            await _postRepository.AddAsync(post);

            return post;
        }

        public async Task<PostDto> GetPostById(Guid id,Guid? userId)
        {
            var post = await _postRepository.GetAll().Where(x => x.Id == id).Include(x => x.User)
                .Include(x=>x.Comments).ThenInclude(x=>x.Replies).ThenInclude(x=>x.ParentReply)
                .ThenInclude(x=>x.User)
                .Include(x=>x.Comments).ThenInclude(x=>x.Likes)
                .Select(x => new PostDto
                {
                    Id = x.Id,
                    Content = x.Content,
                    LinkUrl = x.LinkUrl,
                    ContentType = x.ContentType,
                    UserPostVote = x.Votes.FirstOrDefault(u=>u.IsDeleted==false && u.UserId==userId && u.PostId==x.Id ),
                    ContentPath = BlobService.BlobService.GetImageUrl(x.MediaContentPath),
                    CreatedDateTime = x.CreatedDate,
                    Community = new PostCommunityDto
                    {
                        Id = x.Community.Id,
                        Name = x.Community.Name,
                        LogoPath = BlobService.BlobService.GetImageUrl(x.Community.LogoPath)
                    },
                    UserInfo = new PostUserDto
                    {
                        Id = x.User.Id,
                        UserName = x.User.Username,
                        ProfileImagePath = BlobService.BlobService.GetImageUrl(x.User.ProfileImagePath)
                    },
                    Comments = x.Comments.Where(c=>c.IsDeleted==false).Select(c => new CommentDto
                    {
                        Id = c.Id,
                        Content = c.Content,
                        IsLoggedComment = c.UserId == userId,
                        IsLoggedLiked = c.Likes.Any(w=>w.IsDeleted==false && w.UserId==userId),
                        CreatedDateTime = c.CreatedDate,
                        LikeCount = c.Likes.Count(l=>l.IsDeleted==false),
                        CommentUserInfo = new CommentUserDto
                        {
                            Id = c.User.Id,
                            UserName = c.User.Username,
                            ProfileImagePath = BlobService.BlobService.GetImageUrl(c.User.ProfileImagePath)
                        },
                        Replies = c.Replies.Where(r=>r.IsDeleted==false).Select(r => new ReplyDto
                        {
                            Id = r.Id,
                            Content = r.Content,
                            Parent = new ParentDto
                            {
                                ParentReplyUserName = r.ParentId != null ? r.ParentReply.User.Username : null,
                                UserId = r.ParentId != null ? r.ParentReply.User.Id : (Guid?) null
                            }  ,
                            IsLoggedReply = r.UserId==userId,
                            IsLoggedLiked = r.Likes.Any(q=>q.IsDeleted==false && q.UserId==userId),
                            CreatedDateTime = r.CreatedDate,
                            LikeCount = r.Likes.Count(l=>l.IsDeleted==false),
                            ReplyUserInfo = new ReplyUserDto
                            {
                                Id = r.User.Id,
                                ProfileImagePath = BlobService.BlobService.GetImageUrl(r.User.ProfileImagePath),
                                UserName = r.User.Username
                            }
                        }).ToList()
                    }).ToList()
                }).FirstOrDefaultAsync();

            var dislikes = await _postVoteRepository.GetAll().Where(x => x.IsDeleted == false && x.PostId==post.Id && x.Value == -1).ToListAsync();
            var likes = await _postVoteRepository.GetAll().Where(x => x.IsDeleted == false && x.PostId==post.Id && x.Value == 1).ToListAsync();
            var voteCount = likes.Count - dislikes.Count;
            post.VoteCount = voteCount;
            return post;
        }

        public async Task Delete(Guid id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            post.IsDeleted = true;
            await _postRepository.UpdateAsync(post);
        }

        public async Task DeleteModerator(ModeratorDeleteDto input)
        {
            var isModerator = await _communityUserRepository.GetAll()
                .FirstOrDefaultAsync(x =>
                    x.IsDeleted == false && x.IsAdmin && x.UserId == input.ModeratorId &&
                    x.CommunityId == input.CommunityId);
            
            if (isModerator == null)
            {
                throw new Exception("Bu kullanicinin yetkisi yok");
            };

            var post = await _postRepository.GetAll()
                .FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == input.PostId);
            if(post==null) throw new Exception("Post bulunamadi");
            
            post.IsDeleted = true;
            await _postRepository.UpdateAsync(post);

            var model = new ModeratorOperation
            {
                Operation = "POST_DELETED",
                ModeratorId = input.ModeratorId,
                CommunityId = input.CommunityId,
                PostId = input.PostId
            };
            await _moderatorOperationRepository.AddAsync(model);

        }

        public async Task<List<UserPostsDto>> GetUserPosts(IdOrUsernameDto input)
        {
            var result = await _postRepository.GetAll().
                Where(x => x.IsDeleted == false && (x.User.Username == input.Username || x.UserId == input.Id)).
                Include(x=>x.Community).Select(
                x => new UserPostsDto
                {
                    Id = x.Id,
                    Content = x.Content,
                    LinkUrl = x.LinkUrl,
                    UserPostVote = x.Votes.FirstOrDefault(p=>p.IsDeleted==false &&
                                                             (p.UserId == input.Id || p.User.Username == input.Username) && p.PostId==x.Id ),
                    MediaContentPath = x.MediaContentPath == null ? null : BlobService.BlobService.GetImageUrl(x.MediaContentPath),
                    ContentType = x.ContentType,
                    CreatedDateTime = x.CreatedDate,
                    VoteCount = x.Votes.Count(v=>v.IsDeleted==false && v.Value==1) - x.Votes.Count(v=>v.IsDeleted==false && v.Value==-1),
                    CommentsCount = x.Comments.Count,
                    Community = new PostCommunityDto
                    {
                        Id = x.Community.Id,
                        Name = x.Community.Name,
                        LogoPath = BlobService.BlobService.GetImageUrl(x.Community.LogoPath)
                    }
                }).OrderByDescending(x=>x.Id).ToListAsync();
            return result;
        }

        public async Task<PostVote> Vote(CreateVoteDto input)
        {
            var isExist = await _postVoteRepository.GetAll()
                .FirstOrDefaultAsync(x =>
                    x.IsDeleted == false && x.UserId == input.UserId && x.PostId == input.PostId);
            if (isExist != null)
            {
                if (input.Value == 0)
                {
                    isExist.IsDeleted = true;
                    await _postVoteRepository.UpdateAsync(isExist);
                    return isExist;
                }
                isExist.Value = input.Value;
                await _postVoteRepository.UpdateAsync(isExist);
                return isExist;
            }
            var model = new PostVote
            {
                PostId = input.PostId,
                UserId = input.UserId,
                Value = input.Value
            };
            await _postVoteRepository.AddAsync(model);
            // notificaiton
            var user = await _userRepository.GetByIdAsync(input.UserId);
            var post = await _postRepository.GetByIdAsync(input.PostId);
            var community = await _communityRepository.GetByIdAsync(post.CommunityId);
            if (post.UserId == user.Id) return model;
            var notify = new Notification
            {
                TargetId = input.PostId,
                OwnerUserId = post.UserId,
                TargetName = community.Name,
                Type = NotifyContentType.PostVote,
                Content = user.Username + " " + "sallamanı oyladı",
                ImgPath = user.ProfileImagePath
            };
            await _notificationRepository.AddAsync(notify);
            return model;
        }
        
        public async Task<List<GetAllPostDto>> HomePosts(Guid userId)
        {
            var result = await  _communityUserRepository.GetAll().Where(x => x.UserId == userId && x.IsDeleted==false )
                .Include(x => x.Community).ThenInclude(x => x.Posts)
                .Select(x => new Example
                {
                    Posts = x.Community.Posts.Where(p=>p.IsDeleted==false).Select(p => new GetAllPostDto
                    {
                        Id = p.Id,
                        Content = p.Content,
                        ContentType = p.ContentType,
                        LinkUrl = p.LinkUrl,
                        VoteCount = p.Votes.Count(v=>v.IsDeleted==false && v.Value==1) - p.Votes.Count(v=>v.IsDeleted==false && v.Value==-1),
                        UserPostVote = p.Votes.FirstOrDefault(l=>l.UserId==userId && l.IsDeleted==false && l.PostId==p.Id ),
                        MediaContentPath = BlobService.BlobService.GetImageUrl(p.MediaContentPath),
                        CreatedDateTime = p.CreatedDate,
                        Community = new PostCommunityDto
                        {
                            Id = x.Community.Id,
                            Name = x.Community.Name,
                            LogoPath = BlobService.BlobService.GetImageUrl(x.Community.LogoPath)
                        },
                        User = new PostUserDto
                        {    
                            Id = p.User.Id,
                            ProfileImagePath = BlobService.BlobService.GetImageUrl(p.User.ProfileImagePath),
                            UserName = p.User.Username    
                        },
                        CommentsCount = p.Comments.Count(c=>c.IsDeleted==false)
                    }).OrderByDescending(p=>p.Id).ToList()
                }).ToListAsync();

            if (result.Count == 0) return await UnauthorizedHomePosts();
            
            var posts = new List<GetAllPostDto>();
            foreach (var item in result)
            {
                foreach (var post in item.Posts)
                {
                    posts.Add(post);
                }
            }
            return posts;

        }

          public async Task<PagedResultDto<GetAllPostDto>> PagedHomePosts(PaginationParams input)
        {
            var result = await  _communityUserRepository.GetAll().Where(x => x.UserId == input.EntityId && x.IsDeleted==false )
                .Include(x => x.Community).ThenInclude(x => x.Posts)
                .Select(x => new Example
                {
                    Posts = x.Community.Posts.Where(p=>p.IsDeleted==false).Select(p => new GetAllPostDto
                    {
                        Id = p.Id,
                        Content = p.Content,
                        ContentType = p.ContentType,
                        LinkUrl = p.LinkUrl,
                        VoteCount = p.Votes.Count(v=>v.IsDeleted==false && v.Value==1) - p.Votes.Count(v=>v.IsDeleted==false && v.Value==-1),
                        UserPostVote = p.Votes.FirstOrDefault(l=>l.UserId== input.EntityId && l.IsDeleted==false && l.PostId==p.Id ),
                        MediaContentPath = BlobService.BlobService.GetImageUrl(p.MediaContentPath),
                        CreatedDateTime = p.CreatedDate,
                        Community = new PostCommunityDto
                        {
                            Id = x.Community.Id,
                            Name = x.Community.Name,
                            LogoPath = BlobService.BlobService.GetImageUrl(x.Community.LogoPath)
                        },
                        User = new PostUserDto
                        {    
                            Id = p.User.Id,
                            ProfileImagePath = BlobService.BlobService.GetImageUrl(p.User.ProfileImagePath),
                            UserName = p.User.Username    
                        },
                        CommentsCount = p.Comments.Count(c=>c.IsDeleted==false)
                    }).OrderByDescending(p=>p.Id).ToList()
                }).ToListAsync();

            //if (result.Count == 0) return await UnauthorizedHomePosts();
            
            var posts = new List<GetAllPostDto>();
            foreach (var item in result)
            {
                foreach (var post in item.Posts)
                {
                    posts.Add(post);
                }
            }
            var aa = posts.Skip((input.PageNumber - 1) * input.PageSize).Take(input.PageSize).ToList();
            var hasNext = posts.Skip((input.PageNumber) * input.PageSize).Any();
            var bb = new PagedResultDto<GetAllPostDto> {Results = aa , HasNext = hasNext};
            return bb;

        }

        public async Task<List<GetAllPostDto>> UnauthorizedHomePosts()
        {
            var result = await _postRepository.GetAll().Where(x => x.IsDeleted == false)
                .Include(x=>x.User)
                .Include(x=>x.Community).Select(
                    x => new GetAllPostDto
                    {
                        Id = x.Id,
                        Content = x.Content,
                        LinkUrl = x.LinkUrl,
                        VoteCount = x.Votes.Count(v=>v.IsDeleted==false && v.Value==1) - x.Votes.Count(v=>v.IsDeleted==false && v.Value==-1),
                        MediaContentPath = BlobService.BlobService.GetImageUrl(x.MediaContentPath),
                        ContentType = x.ContentType,
                        CreatedDateTime = x.CreatedDate,
                        CommentsCount = x.Comments.Count,
                        Community = new PostCommunityDto
                        {
                            Id = x.Community.Id,
                            Name = x.Community.Name,
                            LogoPath = BlobService.BlobService.GetImageUrl(x.Community.LogoPath)
                        },
                        User = new PostUserDto
                        {
                            Id = x.User.Id,
                            UserName = x.User.Username,
                            ProfileImagePath = BlobService.BlobService.GetImageUrl(x.User.ProfileImagePath)
                        }
                    }).ToListAsync();
            return result;
        }
        
        public async Task<PagedResultDto<GetAllPostDto>> PagedUnauthorizedHomePosts(PaginationParams input)
        {
            var result = await _postRepository.GetAll().Where(x => x.IsDeleted == false)
                .Include(x=>x.User)
                .Include(x=>x.Community).Select(
                    x => new GetAllPostDto
                    {
                        Id = x.Id,
                        Content = x.Content,
                        LinkUrl = x.LinkUrl,
                        VoteCount = x.Votes.Count(v=>v.IsDeleted==false && v.Value==1) - x.Votes.Count(v=>v.IsDeleted==false && v.Value==-1),
                        MediaContentPath = BlobService.BlobService.GetImageUrl(x.MediaContentPath),
                        ContentType = x.ContentType,
                        CreatedDateTime = x.CreatedDate,
                        CommentsCount = x.Comments.Count,
                        Community = new PostCommunityDto
                        {
                            Id = x.Community.Id,
                            Name = x.Community.Name,
                            LogoPath = BlobService.BlobService.GetImageUrl(x.Community.LogoPath)
                        },
                        User = new PostUserDto
                        {
                            Id = x.User.Id,
                            UserName = x.User.Username,
                            ProfileImagePath = BlobService.BlobService.GetImageUrl(x.User.ProfileImagePath)
                        }
                    }).Skip((input.PageNumber - 1) * input.PageSize).Take(input.PageSize).ToListAsync();
            var hasNext = await _postRepository.GetAll().Where(x => x.IsDeleted == false).Skip((input.PageNumber) * input.PageSize).AnyAsync();
            var bb = new PagedResultDto<GetAllPostDto> {Results = result , HasNext = hasNext};
            return bb;
        }
    }
}