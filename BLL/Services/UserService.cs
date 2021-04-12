using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using BLL.Validation;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly INotificationSenderService _notificationSender;

        public UserService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
                            IUnitOfWork unitOfWork, IMapper mapper, INotificationSenderService notificationSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationSender = notificationSender;
        }

        public async Task DeleteUserByNickNameAsync(string modelNickName)
        {
            var user = _unitOfWork.ApplicationUserRepository.GetAllWithDetails()
                                                            .FirstOrDefault(u => u.NickName.ToUpper() == modelNickName.ToUpper());
            if (user == null)
                throw new ForumException($"User with nickname {modelNickName} is not found", modelNickName);

            await _unitOfWork.ApplicationUserRepository.DeleteUserByIdAsync(user.Id);
            await _unitOfWork.SaveAsync();
        }


        public IEnumerable<UserModel> GetAll()
        {
            var appUsers = _unitOfWork.ApplicationUserRepository.GetAllWithDetails().ToList();

            return _mapper.Map<IEnumerable<UserModel>>(appUsers);
        }

        public async Task<UserModel> GetByNickNameAsync(string modelNickName)
        {
            var user = await _unitOfWork.ApplicationUserRepository.GetByNickNameWithDetails(modelNickName);

            return _mapper.Map<UserModel>(user);
        }

        public IEnumerable<UserModel> GetLoggedInUsers()
        {
            var users = _unitOfWork.ApplicationUserRepository.GetAllWithDetails().Where(u => u.IsOnline == true).ToList();

            return _mapper.Map<IEnumerable<UserModel>>(users);
        }

        public IEnumerable<UserActivityModel> GetNumberOfMostActiveUsers(int userCount)
        {
            var users = _unitOfWork.ApplicationUserRepository.GetAllWithDetails().ToList();
            var mostActiveUsers = _mapper.Map<IEnumerable<UserActivityModel>>(users).Where(u => u.NickName != null)
                                                                                    .OrderByDescending(c => c.MessagesCount)
                                                                                    .Take(userCount);


            return mostActiveUsers;
        }

        public async Task Login(LoginUserModel loginUserModel)
        {
            var signInTask = _signInManager.PasswordSignInAsync(loginUserModel.Email, loginUserModel.Password, loginUserModel.SaveSession, false);

            if (!signInTask.Result.Succeeded)
                throw new ForumException("Incorrect email or password", "loginUserModel");

            var user = _unitOfWork.ApplicationUserRepository.GetAllWithDetails()
                                  .FirstOrDefault(u => u.NormalizedEmail == loginUserModel.Email.ToUpper());

            if (user != null)
                user.IsOnline = true;

            _unitOfWork.ApplicationUserRepository.UpdateUserStaus(user);

            await _unitOfWork.SaveAsync();

        }

        public async Task LogOutAsync(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            user.IsOnline = false;
            await _userManager.UpdateAsync(user);
            await _signInManager.SignOutAsync();
        }

        public async Task RegisterNewUserAsync(RegisterUserModel registerUserModel)
        {

            if (_unitOfWork.ApplicationUserRepository.GetAllWithDetails()
                                                    .Any(u => u.NickName.ToUpper() == registerUserModel.NickName.ToUpper()))
                throw new ForumException($"Nickname {registerUserModel.NickName} is already used", "Nickname");

            var user = _mapper.Map<RegisterUserModel, ApplicationUser>(registerUserModel);

            var createTask = await _userManager.CreateAsync(user, registerUserModel.Password);

            if (createTask.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
                var notification = new NotificationModel()
                {
                    EmailAdress = user.Email,
                    RecipientName = user.UserProfile.FirstName,
                    EmailSubject = "Registration on Forum",
                    EmailText = $"Hi, {user.UserProfile.FirstName}. You registration on Forum is completed successfully "
                };
                await _notificationSender.SendNotification(notification);
            }
            else
                throw new ForumException("Incorrect input Data", "Input data");
        }

        public async Task RemoveAccountAsync(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            
            if (_userManager.GetRolesAsync(user).Result.Contains("admin"))
                throw new ForumException("Admin account can't be removed", "admin");
            
            var userId = _userManager.GetUserId(claimsPrincipal);
         
            await this.LogOutAsync(claimsPrincipal);
            
            await _unitOfWork.ApplicationUserRepository.DeleteUserByIdAsync(userId);
            
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateUserProfileAsync(UpdateUserModel model, ClaimsPrincipal claimsPrincipal)
        {
            var user = _userManager.GetUserAsync(claimsPrincipal).Result;

            if (model.FirstName != null)
                user.UserProfile.FirstName = model.FirstName;
            
            if (model.LastName != null)
                user.UserProfile.LastName = model.LastName;
            
            if (model.About != null)
                user.UserProfile.About = model.About;

            await _userManager.UpdateAsync(user);

        }
    }
}
