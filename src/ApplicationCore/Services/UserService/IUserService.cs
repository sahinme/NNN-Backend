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
        Task<UserDto> GetUserById(int id);
        Task<UserDto> GetByUsername(string username);
        Task UpdateUser(UpdateUserDto input);
        Task<bool> Login(LoginDto input);
        Task DeleteUser(long id);
        Task<CommunityUser> JoinCommunity(long userId,long communityId);
        Task LeaveFromCommunity(long userId,long communityId);
        Task ModeratorRejectedJoin(ModeratorRejected input);
        Task<List<GetAllCommunityDto>> GetUserCommunities(long userId);
        Task<bool> VerifyEmail(string verificationCode);

    }
}