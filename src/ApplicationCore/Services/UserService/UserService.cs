using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Nnn.ApplicationCore.Entities.Users;
using Microsoft.Nnn.ApplicationCore.Interfaces;
using Microsoft.Nnn.ApplicationCore.Services.PasswordHasher;
using Nnn.ApplicationCore.Services.UserService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Services.UserService
{
    public class UserService:IUserService
    {
        private readonly IAsyncRepository<User> _userRepository;

        public UserService(IAsyncRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }
        
        public async Task<User> CreateUser(CreateUserDto input)
        {
            var user = new User
            {
                Name = input.Name,
                Surname = input.Surname,
                EmailAddress = input.EmailAddress,
                Gender = input.Gender,
                Username = input.Username
            };
            var hashedPassword = SecurePasswordHasherHelper.Hash(input.Password);
            user.Password = hashedPassword;
            await _userRepository.AddAsync(user);
            return user;
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user;
        }

        public async Task UpdateUser(UpdateUserDto input)
        {
            var user = await _userRepository.GetByIdAsync(input.Id);
            user.Gender = input.Gender;
            user.Name = input.Name;
            user.Surname = input.Surname;
            user.EmailAddress = input.EmailAddress;
            user.Username = input.Username;
            
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