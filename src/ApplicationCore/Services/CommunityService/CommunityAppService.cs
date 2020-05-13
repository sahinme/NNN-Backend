using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Nnn.ApplicationCore.Entities.Communities;
using Microsoft.Nnn.ApplicationCore.Entities.CommunityUsers;
using Microsoft.Nnn.ApplicationCore.Entities.Posts;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Microsoft.Nnn.ApplicationCore.Services.BlobService;
using Microsoft.Nnn.ApplicationCore.Services.CommunityService.Dto;
using Microsoft.Nnn.ApplicationCore.Services.PostAppService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.CommunityService
{
    public class CommunityAppService:ICommunityAppService
    {
        private readonly IAsyncRepository<Community> _communityRepository;
        private readonly IAsyncRepository<CommunityUser> _communityUserRepository;
        private readonly IAsyncRepository<Post> _postRepository;
        private readonly IBlobService _blobService;

        public CommunityAppService(IAsyncRepository<Community> communityRepository,IBlobService blobService,
            IAsyncRepository<Post> postRepository,IAsyncRepository<CommunityUser> communityUserRepository)
        {
            _communityRepository = communityRepository;
            _blobService = blobService;
            _postRepository = postRepository;
            _communityUserRepository = communityUserRepository;
        }
        
        public async Task<Community> CreateCommunity(CreateCommunity input)
        {
            var model = new Community
            {
                Name = input.Name,
                Description = input.Description
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
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    MemberCount = x.Users.Count,
                    LogoPath = BlobService.BlobService.GetImageUrl(x.LogoPath)
                }).ToListAsync();
            return result;
        }

        public async Task<Community> Update(UpdateCommunity input)
        {
            var isAdmin = await _communityUserRepository.GetAll()
                .FirstOrDefaultAsync(x =>
                    x.IsDeleted == false && x.IsAdmin && x.UserId == input.ModeratorId && x.CommunityId == input.Id);

            if (isAdmin == null)
            {
                throw new Exception("Bu kullanıcının yetkisi yok");
            }
            
            var community = await _communityRepository.GetByIdAsync(input.Id);
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

        public async Task<List<GetAllCommunityDto>> OfModerators(long userId)
        {
            var result = await _communityUserRepository.GetAll()
                .Where(x => x.IsDeleted == false && x.IsAdmin && x.UserId == userId)
                .Include(x => x.User).Include(x => x.Community).ThenInclude(x => x.Users)
                .Select(x => new GetAllCommunityDto
                {
                    Id = x.Community.Id,
                    Description = x.Community.Description,
                    LogoPath = BlobService.BlobService.GetImageUrl(x.Community.LogoPath),
                    MemberCount = x.Community.Users.Count(m => m.IsDeleted == false),
                    Name = x.Community.Name
                }).ToListAsync();
            return result;
        }

        public async Task<List<CommunityUserDto>> Users(long id)
        {
            var result = await _communityRepository.GetAll().Where(x => x.Id == id && x.IsDeleted == false)
                .Include(x => x.Users)
                .ThenInclude(x => x.User).Select(x => new CommunityDto
                {
                    Members = x.Users.Where(m=>m.IsDeleted==false).Select(m => new CommunityUserDto
                    {
                        Id = m.User.Id,
                        PostCount = m.User.Posts.Count(p=>p.IsDeleted==false),
                        Username = m.User.Username,
                        ProfileImg = BlobService.BlobService.GetImageUrl(m.User.ProfileImagePath)
                    }).ToList()
                }).FirstOrDefaultAsync();
            var users = result.Members;
            return users;
        }

        public async Task<CommunityDto> GetById(long id,long? userId)
        {
            var result = await _communityRepository.GetAll().Where(x => x.Id == id && x.IsDeleted == false)
                .Include(x => x.Users)
                .ThenInclude(x => x.User).Select(x => new CommunityDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    LogoPath = BlobService.BlobService.GetImageUrl(x.LogoPath),
                    CoverImagePath = BlobService.BlobService.GetImageUrl(x.CoverImagePath),
                    CreatedDate = x.CreatedDate,
                    Moderators = x.Users.Where(q=>q.IsDeleted==false && q.Suspended==false && q.IsAdmin==true ).Select(q=>new CommunityUserDto
                    {
                        Id = q.User.Id,
                        Username = q.User.Username,
                        ProfileImg = BlobService.BlobService.GetImageUrl(q.User.ProfileImagePath)
                    }).ToList(),
                    Members = x.Users.Where(m=>m.IsDeleted==false).Select(m => new CommunityUserDto
                    {
                        Id = m.User.Id,
                        Username = m.User.Username,
                        ProfileImg = BlobService.BlobService.GetImageUrl(m.User.ProfileImagePath)
                    }).ToList()
                }).FirstOrDefaultAsync();

            var posts = await _postRepository.GetAll().Where(x => x.IsDeleted==false && x.CommunityId == id)
                .Include(x => x.Comments).ThenInclude(x => x.Replies)
                .Include(x => x.User).Select(x => new CommunityPostDto
                {
                    Id = x.Id,
                    Content = x.Content,
                    LinkUrl = x.LinkUrl,
                    MediaContentPath = BlobService.BlobService.GetImageUrl(x.MediaContentPath),
                    ContentType = x.ContentType,
                    CreatedDateTime = x.CreatedDate,
                    UserPostVote = x.Votes.FirstOrDefault(p=>p.IsDeleted==false &&
                                                             p.UserId == userId  && p.PostId==x.Id ),
                    VoteCount = x.Votes.Count(v=>v.IsDeleted==false && v.Value==1) - x.Votes.Count(v=>v.IsDeleted==false && v.Value==-1),
                    CommentsCount = x.Comments.Count,
                    User = new PostUserDto
                    {
                        Id = x.User.Id,
                        ProfileImagePath = BlobService.BlobService.GetImageUrl(x.User.ProfileImagePath),
                        UserName = x.User.Username
                    },
                }).ToListAsync();
            result.Posts = posts;
            return result;
        }

        public async Task<List<GetAllCommunityDto>> GetPopulars(long? userId)
        {
            var result = await _communityRepository.GetAll().Where(x => x.IsDeleted == false)
                .Include(x => x.Users).ThenInclude(x => x.User)
                .Select(x=> new GetAllCommunityDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    MemberCount = x.Users.Count(m=>m.IsDeleted==false),
                    LogoPath = x.LogoPath == null ? null : BlobService.BlobService.GetImageUrl(x.LogoPath),
                    IsUserJoined = x.Users.Any(u=>u.IsDeleted==false && u.UserId==userId )
                }).OrderByDescending(x=>x.MemberCount).Take(5).ToListAsync();
            return result;

        }
    }
}