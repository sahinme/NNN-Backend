using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Nnn.ApplicationCore.Entities;
using Microsoft.Nnn.ApplicationCore.Entities.Categories;
using Microsoft.Nnn.ApplicationCore.Entities.Communities;
using Microsoft.Nnn.ApplicationCore.Entities.CommunityUsers;
using Microsoft.Nnn.ApplicationCore.Entities.Likes;
using Microsoft.Nnn.ApplicationCore.Entities.PostCategories;
using Microsoft.Nnn.ApplicationCore.Entities.Posts;
using Microsoft.Nnn.ApplicationCore.Entities.PostTags;
using Microsoft.Nnn.ApplicationCore.Entities.Unlikes;
using Microsoft.Nnn.ApplicationCore.Entities.Users;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Microsoft.Nnn.ApplicationCore.Services.BlobService;
using Microsoft.Nnn.ApplicationCore.Services.CategoryService.Dto;
using Microsoft.Nnn.ApplicationCore.Services.CommentService.Dto;
using Microsoft.Nnn.ApplicationCore.Services.Dto;
using Microsoft.Nnn.ApplicationCore.Services.PostAppService.Dto;
using Microsoft.Nnn.ApplicationCore.Services.ReplyService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.PostService
{
    public class PostAppService:IPostAppService
    {
        private readonly IAsyncRepository<Post> _postRepository;
        private readonly IAsyncRepository<PostLike> _postLikeRepository;
        private readonly IAsyncRepository<Unlike> _unlikeRepository;
        private readonly IAsyncRepository<User> _userRepository;
        private readonly IAsyncRepository<CommunityUser> _communityUserRepository;
        private readonly IAsyncRepository<PostCategory> _postCategoryRepository;
        private readonly IAsyncRepository<Tag> _tagRepository;
        private readonly IAsyncRepository<PostTag> _postTagRepository;
        private readonly IBlobService _blobService;

        public PostAppService(IAsyncRepository<Post> postRepository,IAsyncRepository<PostCategory> postCategoryRepository,
            IAsyncRepository<PostTag> postTagRepository, IAsyncRepository<Tag> tagRepository,
            IBlobService blobService,IAsyncRepository<PostLike> postLikeRepository,IAsyncRepository<Unlike> unlikeRepository,
            IAsyncRepository<User> userRepository,IAsyncRepository<CommunityUser> communityUserRepository)
        {
            _postRepository = postRepository;
            _postCategoryRepository = postCategoryRepository;
            _postTagRepository = postTagRepository;
            _tagRepository = tagRepository;
            _blobService = blobService;
            _unlikeRepository = unlikeRepository;
            _postLikeRepository = postLikeRepository;
            _userRepository = userRepository;
            _communityUserRepository = communityUserRepository;
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
                post.ContentType = ContentType.Image;
            }

            if (input.ContentType == ContentType.Link)
            {
                post.LinkUrl = input.LinkUrl;
            }
            await _postRepository.AddAsync(post);
            
//            foreach (var item in input.CategoryIds)
//            {
//                var result = new PostCategory
//                {
//                    CategoryId = item,
//                    PostId = post.Id
//                };
//                await _postCategoryRepository.AddAsync(result);
//            }
//
//            foreach (var tagItem in input.Tags)
//            {
//                var item = new PostTag();
//                var isExist = await _tagRepository.GetAll().Where(x => String.Equals(x.Text, tagItem, StringComparison.CurrentCultureIgnoreCase))
//                    .FirstOrDefaultAsync();
//                if (isExist != null)
//                {
//                    item.PostId = post.Id;
//                    item.TagId = isExist.Id;
//                    await _postTagRepository.AddAsync(item);
//                }
//                else
//                {
//                    var newTag = new Tag
//                    {
//                        Text = tagItem
//                    };
//                    await _tagRepository.AddAsync(newTag);
//                    item.PostId = post.Id;
//                    item.TagId = newTag.Id;
//                    await _postTagRepository.AddAsync(item);
//                }
//            }
            return post;
        }

        public async Task<PostDto> GetPostById(long id,long? userId)
        {
            var post = await _postRepository.GetAll().Where(x => x.Id == id).Include(x => x.User)
                .Include(x=>x.Comments).ThenInclude(x=>x.Replies).ThenInclude(x=>x.Likes)
                .Include(x=>x.Comments).ThenInclude(x=>x.Likes)
                .Include(x=>x.Tags).ThenInclude(x=>x.Tag)
                .Select(x => new PostDto
                {
                    Id = x.Id,
                    Content = x.Content,
                    LinkUrl = x.LinkUrl,
                    ContentType = x.ContentType,
                    ContentPath = BlobService.BlobService.GetImageUrl(x.MediaContentPath),
                    CreatedDateTime = x.CreatedDate,
                    LikeCount = x.Likes.Count(l=>l.IsDeleted==false),
                    UnlikeCount = x.Unlikes.Count(u=>u.IsDeleted==false),
                    Community = new PostCommunityDto
                    {
                        Id = x.Community.Id,
                        Name = x.Community.Name
                    },
                    UserInfo = new PostUserDto
                    {
                        Id = x.User.Id,
                        UserName = x.User.Username,
                        ProfileImagePath = BlobService.BlobService.GetImageUrl(x.User.ProfileImagePath)
                    },
                    Comments = x.Comments.Select(c => new CommentDto
                    {
                        Id = c.Id,
                        Content = c.Content,
                        CreatedDateTime = c.CreatedDate,
                        LikeCount = c.Likes.Count,
                        CommentUserInfo = new CommentUserDto
                        {
                            Id = c.User.Id,
                            UserName = c.User.Username,
                            ProfileImagePath = BlobService.BlobService.GetImageUrl(c.User.ProfileImagePath)
                        },
                        Replies = c.Replies.Select(r => new ReplyDto
                        {
                            Id = r.Id,
                            Content = r.Content,
                            CreatedDateTime = r.CreatedDate,
                            LikeCount = r.Likes.Count,
                            ReplyUserInfo = new ReplyUserDto
                            {
                                Id = r.User.Id,
                                ProfileImagePath = BlobService.BlobService.GetImageUrl(r.User.ProfileImagePath),
                                UserName = r.User.Username
                            }
                        }).ToList()
                    }).ToList()
                }).FirstOrDefaultAsync();
            if (userId == null) return post;
                var isLiked = await _postLikeRepository.GetAll().Where(x => x.IsDeleted==false && x.UserId == userId && x.PostId==id )
                    .FirstOrDefaultAsync();
                post.IsLiked = isLiked;
            return post;
        }

        public async Task Delete(long id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            post.IsDeleted = true;
            await _postRepository.UpdateAsync(post);
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
                    MediaContentPath = x.MediaContentPath == null ? null : BlobService.BlobService.GetImageUrl(x.MediaContentPath),
                    ContentType = x.ContentType,
                    CreatedDateTime = x.CreatedDate,
                    CommentsCount = x.Comments.Count,
                    LikesCount = x.Likes.Count(l=>l.IsDeleted==false),
                    UnlikesCount = x.Unlikes.Count,
                    Community = new PostCommunityDto
                    {
                        Id = x.Community.Id,
                        Name = x.Community.Name
                    }
                }).ToListAsync();
            return result;
        }

        public async Task<PostLike> LikePost(CreateLikeDto input)
        {
            var alreadyLiked = await _postLikeRepository.GetAll()
                .FirstOrDefaultAsync(x => x.IsDeleted==false && x.UserId == input.UserId && x.PostId == input.PostId);
            if(alreadyLiked!=null) throw new Exception("Bu islem zaten yapilmis");
            
            var isUnliked = await _unlikeRepository.GetAll()
                .Where(x => x.UserId == input.UserId && x.PostId == input.PostId)
                .FirstOrDefaultAsync();

            if (isUnliked != null)
            {
                isUnliked.IsDeleted = true;
                await _unlikeRepository.UpdateAsync(isUnliked);
            }
            
            var model = new PostLike
            {
                UserId = input.UserId,
                PostId = input.PostId
            };
            await _postLikeRepository.AddAsync(model);
            return model;
        }
        
        public async Task ConvertLike(long id)
        {
            var result = await _postLikeRepository.GetByIdAsync(id);
            if (result == null) throw new Exception("Boyle bir islem yok => "+id) ;
            result.IsDeleted = true;
            await _postLikeRepository.UpdateAsync(result);
        }
        
        public async Task<Unlike> UnlikePost(CreateLikeDto input)
        {
            var alreadyUnliked = await _unlikeRepository.GetAll()
                .FirstOrDefaultAsync(x => x.UserId == input.UserId && x.PostId == input.PostId);
            if(alreadyUnliked!=null) throw new Exception("Bu islem zaten yapilmis");
            
            var isLiked = await _postLikeRepository.GetAll()
                .Where(x => x.UserId == input.UserId && x.PostId == input.PostId)
                .FirstOrDefaultAsync();

            if (isLiked != null)
            {
                isLiked.IsDeleted = true;
                await _postLikeRepository.UpdateAsync(isLiked);
            }
            
            var model = new Unlike
            {
                UserId = input.UserId,
                PostId = input.PostId
            };
            await _unlikeRepository.AddAsync(model);
            
            return model;
        }

        public async Task<Example> HomePosts(long userId)
        {
            var result = await  _communityUserRepository.GetAll().Where(x => x.UserId == userId)
                .Include(x => x.Community).ThenInclude(x => x.Posts)
                .ThenInclude(x=>x.Likes)
                .Select(x => new Example
                {
                    Data = x.Community.Posts.Select(p => new GetAllPostDto
                    {
                        Id = p.Id,
                        Content = p.Content,
                        ContentType = p.ContentType,
                        LinkUrl = p.LinkUrl,
                        MediaContentPath = BlobService.BlobService.GetImageUrl(p.MediaContentPath),
                        CreatedDateTime = p.CreatedDate,
                        LikesCount = p.Likes.Count(l=>l.IsDeleted==false),
                        IsLiked = p.Likes.FirstOrDefault(a=>a.UserId==userId && a.IsDeleted==false),
                        Community = new PostCommunityDto
                        {
                            Id = x.Community.Id,
                            Name = x.Community.Name
                        },
                        User = new PostUserDto
                        {    
                            Id = x.User.Id,
                            ProfileImagePath = BlobService.BlobService.GetImageUrl(x.User.ProfileImagePath),
                            UserName = x.User.Username
                        },
                        CommentsCount = p.Comments.Count
                    }).OrderByDescending(p=>p.Id).ToList()
                }).FirstOrDefaultAsync();
            return result;

        }

        public async Task<List<GetAllPostDto>> UnauthorizedHomePosts()
        {
            var result = await _postRepository.GetAll().Where(x => x.IsDeleted == false)
                .OrderByDescending(x=>x.Likes.Count)
                .Include(x=>x.User)
                .Include(x=>x.Community).Select(
                    x => new GetAllPostDto
                    {
                        Id = x.Id,
                        Content = x.Content,
                        LinkUrl = x.LinkUrl,
                        MediaContentPath = BlobService.BlobService.GetImageUrl(x.MediaContentPath),
                        ContentType = x.ContentType,
                        CreatedDateTime = x.CreatedDate,
                        CommentsCount = x.Comments.Count,
                        LikesCount = x.Likes.Count(l=>l.IsDeleted==false),
                        UnlikesCount = x.Unlikes.Count,
                        Community = new PostCommunityDto
                        {
                            Id = x.Community.Id,
                            Name = x.Community.Name
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
    }
}