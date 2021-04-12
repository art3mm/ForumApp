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
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessageService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<int> AddAsync(MessageModel model, ClaimsPrincipal claims)
        {
            if (model == null)
                throw new ForumException("Model can't be null", "MessageModel");

            if(model.TopicId==default)
                throw new ForumException("Topic title wasn't selected", "TopicId");

            var topic = await _unitOfWork.TopicRepository.GetByIdAsync(model.TopicId);

            if (topic==null)
                throw new ForumException($"No topic with Id={model.TopicId}", "TopicId");

            if (String.IsNullOrEmpty(model.Text))
                throw new ForumException("Message Text can't be empty", "Text");

            var user = _userManager.GetUserAsync(claims).Result;

            model.ApplicationUserId = _userManager.GetUserId(claims);

            model.CreateDate = DateTime.Now;

            var message = _mapper.Map<Message>(model);

            await _unitOfWork.MessageRepository.AddAsync(message);

            await _unitOfWork.SaveAsync();

            return message.Id;
        }

        public async Task DeleteByIdAsync(int modelId, ClaimsPrincipal claims)
        {
            var user = _userManager.GetUserAsync(claims).Result;

            var message = _unitOfWork.MessageRepository.FindAll().FirstOrDefault(m => m.Id == modelId);

            if (message == null)
                throw new ForumException("No message was found with such Id", "modelId");

            if(message.ApplicationUserId!=user.Id)
                throw new ForumException("No message was found for current User", "ApplicationUserId");

            await _unitOfWork.MessageRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public IEnumerable<MessageModel> GetAll()
        {
            var messages = _unitOfWork.MessageRepository.FindAll();
            return _mapper.Map<IEnumerable<MessageModel>>(messages);
        }

        public async Task<MessageModel> GetByIdAsync(int id)
        {
            var message = await _unitOfWork.MessageRepository.GetByIdAsync(id);

            if (message == null)
                throw new ForumException("No message was found with such Id", "Message.Id");

            return _mapper.Map<MessageModel>(message);
        }

        public IEnumerable<CurrentUserMessageModel> GetCurrentUsersMessages(ClaimsPrincipal claims)
        {
            var userId = _userManager.GetUserId(claims);
            var userMessages = _unitOfWork.MessageRepository.GetAllWithDetails().Where(u => u.ApplicationUserId == userId)
                                                                            .ToList();

            return _mapper.Map<IEnumerable<CurrentUserMessageModel>>(userMessages);
        }

        public IEnumerable<MessageModel> SearchMessagesByFilter(FilterSearchModel searchModel)
        {
            var messages = _unitOfWork.MessageRepository.GetAllWithDetails();

            if (!String.IsNullOrEmpty(searchModel.UserNickName))
                messages = messages.Where(u => u.ApplicationUser.NickName.ToUpper() == searchModel.UserNickName.ToUpper());

            if (!String.IsNullOrEmpty(searchModel.TopicTitle))
                messages = messages.Where(t => t.Topic.Title.ToUpper() == searchModel.TopicTitle.ToUpper());

            if (!String.IsNullOrEmpty(searchModel.KeyWord))
                messages = messages.Where(t => t.Text.ToUpper().Contains(searchModel.KeyWord.ToUpper()));

            return _mapper.Map<IEnumerable<MessageModel>>(messages);


        }

        public async Task UpdateAsync(MessageModel model, ClaimsPrincipal claims)
        {
            if (model==null)
                throw new ForumException("Model can't be null", "MessageModel");

            var message = await _unitOfWork.MessageRepository.GetByIdAsync(model.Id);

            if (message == null)
                throw new ForumException("No message was found with such Id", "Message.Id");

            var userId = _userManager.GetUserId(claims);

            if(message.ApplicationUserId!=userId)
                throw new ForumException("No message was found for current User", "ApplicationUserId");

            if (String.IsNullOrEmpty(model.Text))
                throw new ForumException("Message Text can't be empty", "Text");

            message.Text = model.Text;
            message.CreateDate = DateTime.Now;

            _unitOfWork.MessageRepository.Update(message);

            await _unitOfWork.SaveAsync();
        }
    }
}
