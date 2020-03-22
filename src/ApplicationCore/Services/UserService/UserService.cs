using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Nnn.ApplicationCore.Entities.Users;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Microsoft.Nnn.ApplicationCore.Services.BlobService;
using Microsoft.Nnn.ApplicationCore.Services.PasswordHasher;
using Nnn.ApplicationCore.Services.UserService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.UserService
{
    public class UserService:IUserService
    {
        private readonly IAsyncRepository<User> _userRepository;
        private readonly IBlobService _blobService;

        public UserService(IAsyncRepository<User> userRepository,IBlobService blobService)
        {
            _userRepository = userRepository;
            _blobService = blobService;
        }
        
        public async Task<User> CreateUser(CreateUserDto input)
        {
            var user = new User
            {
                Username = input.Username,
                EmailAddress = input.EmailAddress,
                Gender = input.Gender,
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
                ProfileImagePath = BlobService.BlobService.GetImageUrl(x.ProfileImagePath)
            }).FirstOrDefaultAsync();
            return user;
        }

        public async Task UpdateUser(UpdateUserDto input)
        {
            var user = await _userRepository.GetByIdAsync(input.Id);
            user.Gender = input.Gender;
            user.EmailAddress = input.EmailAddress;
            user.Username = input.Username;

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
                throw new Exception("There is no user!");
            }
            var decodedPassword = SecurePasswordHasherHelper.Verify(input.Password, user.Password);
            if (!decodedPassword)
            {
                return false;
            }

            return true;
        }

        public async Task DeleteUser(long id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            user.IsDeleted = true;
            await _userRepository.UpdateAsync(user);
        }
    }
}