using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Nnn.ApplicationCore.Entities.Categories;
using Microsoft.Nnn.ApplicationCore.Entities.Communities;
using Microsoft.Nnn.ApplicationCore.Entities.CommunityUsers;
using Microsoft.Nnn.ApplicationCore.Entities.Posts;
using Microsoft.Nnn.ApplicationCore.Entities.Users;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Microsoft.Nnn.ApplicationCore.Services.BlobService;
using Microsoft.Nnn.ApplicationCore.Services.CommunityService.Dto;
using Microsoft.Nnn.ApplicationCore.Services.Dto;
using Microsoft.Nnn.ApplicationCore.Services.PostAppService.Dto;
using Microsoft.Nnn.ApplicationCore.Services.UserService;

namespace Microsoft.Nnn.ApplicationCore.Services.CommunityService
{
    public class CommunityAppService:ICommunityAppService
    {
        private readonly IAsyncRepository<Community> _communityRepository;
        private readonly IAsyncRepository<CommunityUser> _communityUserRepository;
        private readonly IAsyncRepository<Category> _categoryRepository;
        private readonly IAsyncRepository<Post> _postRepository;
        private readonly IAsyncRepository<User> _userRepository;
        private readonly IBlobService _blobService;

        public CommunityAppService(IAsyncRepository<Community> communityRepository,IBlobService blobService,
            IAsyncRepository<Post> postRepository,IAsyncRepository<CommunityUser> communityUserRepository,
            IAsyncRepository<User> userRepository,IAsyncRepository<Category> categoryRepository)
        {
            _communityRepository = communityRepository;
            _blobService = blobService;
            _postRepository = postRepository;
            _userRepository = userRepository;
            _communityUserRepository = communityUserRepository;
            _categoryRepository = categoryRepository;
        }
        
        public async Task<Community> CreateCommunity(CreateCommunity input)
        {
            var slug = input.Name.GenerateSlug();

            var isExist =  _communityRepository.GetAll().Any(x => x.Slug == slug);
            if (isExist)
            {
                throw new Exception("Böyle bir isimde topluluk var");
            }

            var category = await _categoryRepository.GetAll().FirstOrDefaultAsync(x => x.Slug == input.CatSlug);
            var model = new Community
            {
                Name = input.Name,
                Description = input.Description,
                CategoryId = category.Id,
                Slug = slug
            };
            if (input.LogoFile != null)
            {
                var path = await _blobService.InsertFile(input.LogoFile);
                model.LogoPath = path;
            }
            if (input.CoverImage != null)
            {
                var path = await _blobService.InsertFile(input.CoverImage);
                model.CoverImagePath = path;
            }
            await _communityRepository.AddAsync(model);
            return model;
        }

        public async Task<List<GetAllCommunityDto>> GetAll()
        {
            var result = await _communityRepository.GetAll().Where(x => x.IsDeleted == false)
                .Include(x => x.Users).ThenInclude(x => x.User).Select(x=> new GetAllCommunityDto
                {
                    Slug = x.Slug,
                    Name = x.Name,
                    Description = x.Description,
                    MemberCount = x.Users.Count(m=> !m.IsDeleted),
                    LogoPath = BlobService.BlobService.GetImageUrl(x.LogoPath)
                }).ToListAsync();
            return result;
        }

        public async Task<Community> Update(UpdateCommunity input)
        {
            var isAdmin = await _communityUserRepository.GetAll()
                .FirstOrDefaultAsync(x =>
                    x.IsDeleted == false && x.IsAdmin && x.UserId == input.ModeratorId && x.Community.Slug == input.Slug);

            if (isAdmin == null)
            {
                throw new Exception("Bu kullanıcının yetkisi yok");
            }
            
            var community = await _communityRepository.GetByIdAsync(isAdmin.CommunityId);
            if (input.Name != null) community.Name = input.Name;
            if (input.Description != null) community.Description = input.Description;
            if (input.Logo != null)
            {
                var path = await _blobService.InsertFile(input.Logo);
                community.LogoPath = path;
            }
            if (input.CoverPhoto != null)
            {
                var path = await _blobService.InsertFile(input.CoverPhoto);
                community.CoverImagePath = path;
            }

            await _communityRepository.UpdateAsync(community);
            return community;
        }

        public async Task<List<GetAllCommunityDto>> OfModerators(Guid userId)
        {
            var result = await _communityUserRepository.GetAll()
                .Where(x => x.IsDeleted == false && x.IsAdmin && x.UserId == userId)
                .Include(x => x.User).Include(x => x.Community).ThenInclude(x => x.Users)
                .Select(x => new GetAllCommunityDto
                {
                    Slug = x.Community.Slug,
                    Description = x.Community.Description,
                    LogoPath = BlobService.BlobService.GetImageUrl(x.Community.LogoPath),
                    MemberCount = x.Community.Users.Count(m => m.IsDeleted == false),
                    Name = x.Community.Name
                }).ToListAsync();
            return result;
        }

        public async Task<List<CommunityUserDto>> Users(string slug)
        {
            var result = await _communityRepository.GetAll().Where(x => x.Slug == slug && x.IsDeleted == false)
                .Include(x => x.Users)
                .ThenInclude(x => x.User).Select(x => new CommunityDto
                {
                    Members = x.Users.Where(m=>m.IsDeleted==false).Select(m => new CommunityUserDto
                    {
                        PostCount = m.User.Posts.Count(p=>p.IsDeleted==false),
                        Username = m.User.Username,
                        ProfileImg = BlobService.BlobService.GetImageUrl(m.User.ProfileImagePath)
                    }).ToList()
                }).FirstOrDefaultAsync();
            var users = result.Members;
            return users;
        }

        public async Task<CommunityDto> GetById(string slug,Guid? userId)
        {
            var result = await _communityRepository.GetAll().Where(x => x.Slug == slug && x.IsDeleted == false)
                .Include(x => x.Users)
                .ThenInclude(x => x.User).Select(x => new CommunityDto
                {
                    Slug = x.Slug,
                    Name = x.Name,
                    Description = x.Description,
                    LogoPath = BlobService.BlobService.GetImageUrl(x.LogoPath),
                    CoverImagePath = BlobService.BlobService.GetImageUrl(x.CoverImagePath),
                    CreatedDate = x.CreatedDate,
                    Moderators = x.Users.Where(q=>q.IsDeleted==false && q.Suspended==false && q.IsAdmin ).Select(q=>new CommunityUserDto
                    {
                        Username = q.User.Username,
                        ProfileImg = BlobService.BlobService.GetImageUrl(q.User.ProfileImagePath)
                    }).ToList(),
                    Members = x.Users.Where(m=>m.IsDeleted==false).Select(m => new CommunityUserDto
                    {
                        Username = m.User.Username,
                        ProfileImg = BlobService.BlobService.GetImageUrl(m.User.ProfileImagePath)
                    }).ToList()
                }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<PagedResultDto<CommunityPostDto>> GetPosts(PageDtoCommunity input)
        {
            var posts = await _postRepository.GetAll().Where(x => x.IsDeleted==false && x.Community.Slug == input.Slug)
                .Include(x => x.Comments).ThenInclude(x => x.Replies)
                .Include(x => x.User).Select(x => new CommunityPostDto
                {
                    Id = x.Id,
                    Slug = x.Slug,
                    Content = x.Content,
                    PageNumber = input.PageNumber,
                    LinkUrl = x.LinkUrl,
                    MediaContentPath = BlobService.BlobService.GetImageUrl(x.MediaContentPath),
                    ContentType = x.ContentType,
                    CreatedDateTime = x.CreatedDate,
                    UserPostVote = x.Votes.FirstOrDefault(p=>p.IsDeleted==false &&
                                                             p.UserId == input.UserId  && p.PostId==x.Id ),
                    VoteCount = x.Votes.Count(v=>v.IsDeleted==false && v.Value==1) - x.Votes.Count(v=>v.IsDeleted==false && v.Value==-1),
                    CommentsCount = x.Comments.Count,
                    User = new PostUserDto
                    {
                        ProfileImagePath = BlobService.BlobService.GetImageUrl(x.User.ProfileImagePath),
                        UserName = x.User.Username
                    },
                }).Skip((input.PageNumber - 1) * input.PageSize).Take(input.PageSize).OrderByDescending(x=>x.Id).ToListAsync();
            var hasNext = await _postRepository.GetAll().Where(x => x.IsDeleted==false && x.Community.Slug == input.Slug)
                .Skip((input.PageNumber) * input.PageSize).AnyAsync();
            var bb = new PagedResultDto<CommunityPostDto> {Results = posts ,  HasNext = hasNext};
            return bb;
        }

        public async Task<List<GetAllCommunityDto>> GetPopulars(Guid? userId)
        {
            var result = await _communityRepository.GetAll().Where(x => x.IsDeleted == false)
                .Include(x => x.Users).ThenInclude(x => x.User)
                .Select(x=> new GetAllCommunityDto
                {
                    Slug = x.Slug,
                    Name = x.Name,
                    Description = x.Description,
                    MemberCount = x.Users.Count(m=>m.IsDeleted==false),
                    LogoPath = x.LogoPath == null ? null : BlobService.BlobService.GetImageUrl(x.LogoPath),
                    IsUserJoined = x.Users.Any(u=>u.IsDeleted==false && u.UserId==userId )
                }).OrderByDescending(x=>x.MemberCount).Take(5).ToListAsync();
            return result;

        }
        
        public async Task<List<SearchDto>> Search(string text)
        {
            var coms = await _communityRepository.GetAll().Where(x =>
                    x.IsDeleted == false && x.Name.Contains(text) || x.Description.Contains(text))
                .Select(x => new SearchDto
                {
                    Name = x.Slug,
                    LogoPath = BlobService.BlobService.GetImageUrl(x.LogoPath),
                    MemberCount = x.Users.Count(c => !c.IsDeleted),
                    Type = "community"
                }).ToListAsync();

            var users = await _userRepository.GetAll().Where(x =>
                    x.IsDeleted == false && x.Username.Contains(text) || x.EmailAddress.Contains(text))
                .Select(x => new SearchDto
                {
                    LogoPath = BlobService.BlobService.GetImageUrl(x.ProfileImagePath),
                    Name = x.Username,
                    Type = "user"
                }).ToListAsync();

            var result = coms.Union(users).ToList();
            return result;
        }
    }
}