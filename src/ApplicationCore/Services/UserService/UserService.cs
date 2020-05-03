using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Nnn.ApplicationCore.Entities.CommunityUsers;
using Microsoft.Nnn.ApplicationCore.Entities.Users;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Microsoft.Nnn.ApplicationCore.Services.BlobService;
using Microsoft.Nnn.ApplicationCore.Services.CommunityService.Dto;
using Microsoft.Nnn.ApplicationCore.Services.PasswordHasher;
using Nnn.ApplicationCore.Services.UserService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.UserService
{
    public class UserService:IUserService
    {
        private readonly IAsyncRepository<User> _userRepository;
        private readonly IAsyncRepository<CommunityUser> _communityUserRepository;
        private readonly IBlobService _blobService;

        public UserService(
            IAsyncRepository<User> userRepository,IBlobService blobService,
            IAsyncRepository<CommunityUser> communityUserRepository
            )
        {
            _userRepository = userRepository;
            _blobService = blobService;
            _communityUserRepository = communityUserRepository;
        }
        
        public async Task<User> CreateUser(CreateUserDto input)
        {
            var user = new User
            {
                Username = input.Username,
                EmailAddress = input.EmailAddress,
                Gender = input.Gender,
                Bio = input.Bio,
                VerificationCode = RandomString.GenerateString(35)
            };
            if (input.ProfileImage!=null)
            {
                var imgPath = await _blobService.InsertFile(input.ProfileImage);
                user.ProfileImagePath = imgPath;
            }
            var hashedPassword = SecurePasswordHasherHelper.Hash(input.Password);
            user.Password = hashedPassword;
            await _userRepository.AddAsync(user);
            return user;
        }

        public async Task<UserDto> GetUserById(int id)
        {
            var user = await _userRepository.GetAll().Where(x => x.Id == id).Select(x => new UserDto
            {
                Id = x.Id,
                Username = x.Username,
                EmailAddress = x.EmailAddress,
                Bio = x.Bio,
                Gender = x.Gender,
                ProfileImagePath = x.ProfileImagePath == null ? null : BlobService.BlobService.GetImageUrl(x.ProfileImagePath)
            }).FirstOrDefaultAsync();
            return user;
        }
        
        public async Task<UserDto> GetByUsername(string username)
        {
            var user = await _userRepository.GetAll().Where(x => x.Username == username).Select(x => new UserDto
            {
                Id = x.Id,
                Username = x.Username,
                Bio = x.Bio,
                EmailAddress = x.EmailAddress,
                Gender = x.Gender,
                ProfileImagePath = x.ProfileImagePath == null ? null : BlobService.BlobService.GetImageUrl(x.ProfileImagePath)
            }).FirstOrDefaultAsync();
            return user;    
        }

        public async Task<bool> VerifyEmail(string verificationCode)
        {
            var user = await _userRepository.GetAll().Where(x => x.VerificationCode == verificationCode)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return false;
            }
            user.EmailVerified = true;
            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task UpdateUser(UpdateUserDto input)
        {
            var user = await _userRepository.GetByIdAsync(input.Id);
            
            if (input.Username != null) user.Username = input.Username;
            if (input.EmailAddress != null) user.EmailAddress = input.EmailAddress;
            if (input.Bio != null) user.Bio = input.Bio;
            if (input.Username != null) user.Username = input.Username;
            user.Gender = input.Gender;
            
            if (input.ProfileImage != null)
            {
                var imgPath = await _blobService.InsertFile(input.ProfileImage);
                user.ProfileImagePath = imgPath;
            }
            await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> Login(LoginDto input)
        {
            var user = await _userRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Username == input.Username);
            if (user == null)
            {
                throw new Exception("Böyle bir kullanıcı bulunamadı!");
            }
            var decodedPassword = SecurePasswordHasherHelper.Verify(input.Password, user.Password);
            return decodedPassword;
        }

        public async Task DeleteUser(long id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            user.IsDeleted = true;
            await _userRepository.UpdateAsync(user);
        }

        public async Task<CommunityUser> JoinCommunity(long userId, long communityId)
        {
            var model = new CommunityUser
            {
                UserId = userId,
                CommunityId = communityId
            };
            await _communityUserRepository.AddAsync(model);
            return model;
        }
        
        public async Task LeaveFromCommunity(long userId, long communityId)
        {
            var isExist = await _communityUserRepository.GetAll()
                .Where(x => x.CommunityId == communityId && x.UserId == userId && x.IsDeleted == false )
                .FirstOrDefaultAsync();
            if (isExist == null) throw new Exception("this relation don`t exist");
            isExist.IsDeleted = true;
            await _communityUserRepository.UpdateAsync(isExist);
        }
        
        public async Task<List<GetAllCommunityDto>> GetUserCommunities(long userId)
        {
            var result = await _communityUserRepository.GetAll()
                .Where(x => x.UserId == userId && x.IsDeleted == false && x.Suspended == false)
                .Include(x => x.Community).Select(x => new GetAllCommunityDto
                {
                    Id = x.Community.Id,
                    Name = x.Community.Name,
                    Description = x.Community.Description,
                    MemberCount = x.Community.Users.Count
                }).ToListAsync();
            return result;
        }
        
        
    }
}