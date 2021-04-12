using BLL.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserService
    {
        Task Login(LoginUserModel loginUserModel);
        Task RegisterNewUserAsync(RegisterUserModel registerUserModel);
        Task LogOutAsync(ClaimsPrincipal claimsPrincipal);
        Task DeleteUserByNickNameAsync(string modelNickName);
        Task RemoveAccountAsync(ClaimsPrincipal claimsPrincipal);
        IEnumerable<UserModel> GetAll();
        Task<UserModel> GetByNickNameAsync(string modelNickName);
        IEnumerable<UserModel> GetLoggedInUsers();
        Task UpdateUserProfileAsync(UpdateUserModel model, ClaimsPrincipal claimsPrincipal);
        IEnumerable<UserActivityModel> GetNumberOfMostActiveUsers(int userCount);
    }
}
