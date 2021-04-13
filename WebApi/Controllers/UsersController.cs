using BLL.Interfaces;
using BLL.Models;
using BLL.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;

        public UsersController(IUserService userService, IMessageService messageService)
        {
            _userService = userService;
            _messageService = messageService;
        }
       
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginUserModel loginUserModel)
        {
            if (this.User.Identity.IsAuthenticated)
                return BadRequest($"You are logged in as {this.User.Identity.Name} is Already logged in");
            if (ModelState.IsValid)
            {
                try
                {
                    await _userService.Login(loginUserModel);
                    return Ok($"{loginUserModel.Email} is logged in");
                }
                catch (ForumException ex)
                {
                    return Unauthorized(ex.Message);
                }
            }
            else
                return BadRequest(loginUserModel);
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> RegisterNewUser([FromBody]RegisterUserModel registerUserModel)
        {
            if (this.User.Identity.IsAuthenticated)
                return BadRequest($"You are registered as {this.User.Identity.Name}");
            
            if (ModelState.IsValid)
            {
                try
                {
                    await _userService.RegisterNewUserAsync(registerUserModel);
                    return Created(new Uri($"{ Request.Scheme}://{Request.Host}{Request.Path}{registerUserModel.NickName}", UriKind.Absolute), registerUserModel.NickName);
                }
                catch (ForumException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(registerUserModel);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            await _userService.LogOutAsync(this.User);
            return Ok($"{this.User.Identity.Name} is logged out");
        }

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<UserModel>> GetAllUsers()
        {
            var users = _userService.GetAll().ToList();

            if (users.Count() == 0)
                return Ok("No users were found");

            return Ok(users);
        }

        [Authorize]
        [HttpGet("{userNickname}")]
        public async Task<ActionResult<UserModel>> GetUserByNickName(string userNickname)
        {
            var user = await _userService.GetByNickNameAsync(userNickname);

            if (user == null)
                return NotFound($"No user were found with nick {userNickname}");

            return Ok(user);
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> RemoveAccountAsync()
        {
            try
            {
                var account = this.User.Identity.Name;
                await _userService.RemoveAccountAsync(this.User);
                return Ok($"Account {account} is removed");
            }
            catch (ForumException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> UpdateUserProfileAsync([FromBody]UpdateUserModel userModel)
        {
            try
            {
                await _userService.UpdateUserProfileAsync(userModel, this.User);

                return Ok($"User {this.User.Identity.Name} updatet successfully");
            }
            catch (ForumException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [Authorize]
        [HttpGet("online")]
        public ActionResult<IEnumerable<UserModel>> GetLoggedInUsers()
        {
            var users = _userService.GetLoggedInUsers().ToList();

            if (users.Count() == 0)
                return Ok("No logged in users were found");

            return Ok(users);
        }

        [Authorize(Roles ="admin")]
        [HttpDelete("{modelNickName}")]
        public async Task<ActionResult> DeleteUserByNickNameAsync(string modelNickName)
        {
            try
            {
                await _userService.DeleteUserByNickNameAsync(modelNickName);

                return Ok($"User with nickname {modelNickName} is deleted");
            }
            catch (ForumException ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        [Authorize]
        [HttpGet("activeUsers")]
        public ActionResult<IEnumerable<UserActivityModel>> GetNumberOfMostActiveUsers([FromQuery] int userCount)
        {
            var activeUsers = _userService.GetNumberOfMostActiveUsers(userCount);
           
            if (activeUsers.Count()==0)
                return Ok("No results were found");

            return Ok(activeUsers);
        }

        [Authorize]
        [HttpGet("messages")]
        public ActionResult<IEnumerable<CurrentUserMessageModel>> GetCurrentUserMessages()
        {
            var messages = _messageService.GetCurrentUsersMessages(this.User);
            if (messages.Count() == 0)
                return Ok("No results were found");

            return Ok(messages);
        }


    }
}
