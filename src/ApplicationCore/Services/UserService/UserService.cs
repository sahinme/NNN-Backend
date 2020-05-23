using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Nnn.ApplicationCore.Entities.CommunityUsers;
using Microsoft.Nnn.ApplicationCore.Entities.ModeratorOperations;
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
        private readonly IAsyncRepository<ModeratorOperation> _moderatorOperationRepository;
        private readonly IBlobService _blobService;
        private readonly IEmailSender _emailSender;

        public UserService(
            IAsyncRepository<User> userRepository,IBlobService blobService,
            IAsyncRepository<CommunityUser> communityUserRepository,
            IAsyncRepository<ModeratorOperation> moderatorOperationRepository,
            IEmailSender emailSender
            )
        {
            _userRepository = userRepository;
            _blobService = blobService;
            _communityUserRepository = communityUserRepository;
            _moderatorOperationRepository = moderatorOperationRepository;
            _emailSender = emailSender;
        }
        
        public async Task<User> CreateUser(CreateUserDto input)
        {
            var isUsernameTaken = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Username == input.Username);
            if (isUsernameTaken != null)
            {
                var model = new User();
                model.Username = "Bu kullanıcı adı daha önce alınmış";
                return model;
            }
            
            var isEmailTaken = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.EmailAddress == input.EmailAddress);
            if (isEmailTaken != null)
            {
                var model = new User();
                model.EmailAddress = "Bu E-Posta adresi daha önce alınmış";
                return model;
            }
        
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

        public async Task<UserDto> GetUserById(Guid id)
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

            var isModerator = await _communityUserRepository.GetAll().AnyAsync(x =>
                x.IsDeleted == false && x.Suspended == false && x.IsAdmin && x.UserId == user.Id);
            user.IsModerator = isModerator;
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

        public async Task<bool> SendResetCode(string emailAddress)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.EmailAddress == emailAddress);
                if(user==null) throw new Exception("Kullanıcı bulunamadı");

                var resetCode = RandomString.GenerateString(10);
                user.ResetPasswordCode = resetCode;
                await _userRepository.UpdateAsync(user);
                var message = "Şifre Sıfırlama İçin Doğrulama Kodu: " + resetCode;
                await _emailSender.SendEmail(emailAddress, "Şifre Sıfırlama", message);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
    
        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {
            var user = await _userRepository.GetAll().Where(x =>
                    x.EmailAddress == input.EmailAddress && x.ResetPasswordCode == input.ResetCode)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                throw new Exception("Böyle bir işlem yok");
            }
            
            var hashedPassword = SecurePasswordHasherHelper.Hash(input.NewPassword);
            user.Password = hashedPassword;
            await _userRepository.UpdateAsync(user);
            return true;
        }
        
        public async Task<bool> ChangePassword(ChangePasswordDto input)
        {
            var user = await _userRepository.GetByIdAsync(input.UserId);
            if (user == null)
            {
                throw new Exception("Kullanici bulunamadi");
            }
            var decodedPassword = SecurePasswordHasherHelper.Verify(input.OldPassword, user.Password);
            if(!decodedPassword) throw new Exception("Eski sifre yanlis");
            var hashedPassword = SecurePasswordHasherHelper.Hash(input.NewPassword);
            user.Password = hashedPassword;
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

        public async Task DeleteUser(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            user.IsDeleted = true;
            await _userRepository.UpdateAsync(user);
        }

        public async Task<CommunityUser> JoinCommunity(Guid userId, Guid communityId)
        {
            var isExist = await _communityUserRepository.GetAll()
                .Where(x => x.IsDeleted == false && x.UserId==userId && x.CommunityId==communityId )
                .FirstOrDefaultAsync();
            if (isExist != null)
            {
                throw new Exception("bu islem zaten yapilmis");
            }
            var model = new CommunityUser
            {
                UserId = userId,
                CommunityId = communityId
            };
            await _communityUserRepository.AddAsync(model);
            return model;
        }
        
        public async Task LeaveFromCommunity(Guid userId, Guid communityId)
        {
            var isExist = await _communityUserRepository.GetAll()
                .Where(x => x.CommunityId == communityId && x.UserId == userId && x.IsDeleted == false )
                .FirstOrDefaultAsync();
            if (isExist == null) throw new Exception("this relation don`t exist");
            isExist.IsDeleted = true;
            await _communityUserRepository.UpdateAsync(isExist);
        }
        
        public async Task ModeratorRejectedJoin(ModeratorRejected input)
        {
            var isExist = await _communityUserRepository.GetAll()
                .Where(x => x.CommunityId == input.CommunityId && x.UserId == input.UserId && x.IsDeleted == false )
                .FirstOrDefaultAsync();
            if (isExist == null) throw new Exception("this relation don`t exist");
            isExist.IsDeleted = true;
            await _communityUserRepository.UpdateAsync(isExist);

            var model = new ModeratorOperation
            {
                Operation = "USER_REJECTED",
                CommunityId = input.CommunityId,
                ModeratorId = input.ModeratorId,
                UserId = input.UserId
            };
            await _moderatorOperationRepository.AddAsync(model);
        }
        
        public async Task<List<GetAllCommunityDto>> GetUserCommunities(Guid userId)
        {
            var result = await _communityUserRepository.GetAll()
                .Where(x => x.UserId == userId && x.IsDeleted == false && x.Suspended == false)
                .Include(x => x.Community).Select(x => new GetAllCommunityDto
                {
                    Id = x.Community.Id,
                    Name = x.Community.Name,
                    Description = x.Community.Description,
                    LogoPath = x.Community.LogoPath == null ? null : BlobService.BlobService.GetImageUrl(x.Community.LogoPath),
                    MemberCount = x.Community.Users.Count(m=>m.IsDeleted==false)
                }).ToListAsync();
            return result;
        }
    }
}