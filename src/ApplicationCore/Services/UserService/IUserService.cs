using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Nnn.ApplicationCore.Entities.CommunityUsers;
using Microsoft.Nnn.ApplicationCore.Entities.Users;
using Microsoft.Nnn.ApplicationCore.Services.CommunityService.Dto;
using Nnn.ApplicationCore.Services.UserService.Dto;

namespace Microsoft.Nnn.ApplicationCore.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUser(CreateUserDto input);
        Task<UserDto> GetUserById(Guid id);
        Task<UserDto> GetByUsername(string username);
        Task UpdateUser(UpdateUserDto input);
        Task<bool> Login(LoginDto input);
        Task DeleteUser(Guid id);
        Task<CommunityUser> JoinCommunity(Guid userId,Guid communityId);
        Task LeaveFromCommunity(Guid userId,Guid communityId);
        Task ModeratorRejectedJoin(ModeratorRejected input);
        Task<List<GetAllCommunityDto>> GetUserCommunities(Guid userId);
        Task<bool> VerifyEmail(string verificationCode);
        Task<bool> SendResetCode(string emailAddress);
        Task<bool> ResetPassword(ResetPasswordDto input);
        Task<bool> ChangePassword(ChangePasswordDto input);
    }
}