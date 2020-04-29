using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Nnn.ApplicationCore.Entities;
using Microsoft.Nnn.ApplicationCore.Entities.Categories;
using Microsoft.Nnn.ApplicationCore.Entities.Likes;
using Microsoft.Nnn.ApplicationCore.Entities.PostCategories;
using Microsoft.Nnn.ApplicationCore.Entities.Posts;
using Microsoft.Nnn.ApplicationCore.Entities.PostTags;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Microsoft.Nnn.ApplicationCore.Services.BlobService;
using Microsoft.Nnn.ApplicationCore.Services.CategoryService.Dto;
using Microsoft.Nnn.ApplicationCore.Services.Dto;
using Microsoft.Nnn.ApplicationCore.Services.PostAppService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.PostService
{
    public class PostAppService:IPostAppService
    {
        private readonly IAsyncRepository<Post> _postRepository;
        private readonly IAsyncRepository<Like> _likeRepository;
        private readonly IAsyncRepository<PostCategory> _postCategoryRepository;
        private readonly IAsyncRepository<Tag> _tagRepository;
        private readonly IAsyncRepository<PostTag> _postTagRepository;
        private readonly IBlobService _blobService;

        public PostAppService(IAsyncRepository<Post> postRepository,IAsyncRepository<PostCategory> postCategoryRepository,
            IAsyncRepository<PostTag> postTagRepository, IAsyncRepository<Tag> tagRepository,
            IBlobService blobService,IAsyncRepository<Like> likeRepository)
        {
            _postRepository = postRepository;
            _postCategoryRepository = postCategoryRepository;
            _postTagRepository = postTagRepository;
            _tagRepository = tagRepository;
            _blobService = blobService;
            _likeRepository = likeRepository;
        }
        
        public async Task<Post> CreatePost(CreatePostDto input)
        {
            var post = new Post
            {
               Content = input.Content,
               CommunityId = input.CommunityId,
               UserId = input.UserId
            };
            if (input.ContentFile != null)
            {
                var path = await _blobService.InsertFile(input.ContentFile);
                post.MediaContentPath = path;
                post.ContentType = ContentType.Image;
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

        public async Task<PostDto> GetPostById(long id)
        {
            var post = await _postRepository.GetAll().Where(x => x.Id == id).Include(x => x.User)
                .Include(x=>x.Tags).ThenInclude(x=>x.Tag)
                .Select(x => new PostDto
                {
                    Id = x.Id,
                    Content = x.Content,
                    ContentType = x.ContentType,
                    ContentPath = BlobService.BlobService.GetImageUrl(x.MediaContentPath),
                    CreatedDateTime = x.CreatedDate,
                    UserInfo = new PostUserDto
                    {
                        Id = x.User.Id,
                        UserName = x.User.Username,
                        ProfileImagePath = x.User.ProfileImagePath
                    },
                    Tags = x.Tags.Select(t=>new TagDto
                    {
                        Id = t.Tag.Id,
                        Text = t.Tag.Text
                    }).ToList(),
                }).FirstOrDefaultAsync();
            var likes = await _likeRepository.GetAll()
                .Where(x => x.EntityType == EntityType.Post && x.EntityId == post.Id).CountAsync();
            post.LikeCount = likes;
            return post;
        }
    }
}