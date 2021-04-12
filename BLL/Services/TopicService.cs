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
    public class TopicService : ITopicService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public TopicService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<int> AddAsync(TopicModel model, ClaimsPrincipal claims)
        {
            var user = _userManager.GetUserAsync(claims).Result;

            if (!_userManager.GetRolesAsync(user).Result.Any(r => r == "admin"))
                throw new ForumException("Only admin can add new topic", "claims");

            if (model == null)
                throw new ForumException("Topic can't be null", "TopicModel");

            if (String.IsNullOrEmpty(model.Title))
                throw new ForumException("Topic can't be without title", "TopicModel");

            var topic = _mapper.Map<Topic>(model);
            
            await _unitOfWork.TopicRepository.AddAsync(topic);
            
            await _unitOfWork.SaveAsync();

            return topic.Id;
        }

        public async Task DeleteByIdAsync(int modelId, ClaimsPrincipal claims)
        {
            var user = _userManager.GetUserAsync(claims).Result;

            if (!_userManager.GetRolesAsync(user).Result.Any(r => r == "admin"))
                throw new ForumException("Only admin can delete topic", "claims");

            var topic = _unitOfWork.TopicRepository.FindAllWithDetails().FirstOrDefault(t => t.Id == modelId);

            if (topic == null)
                throw new ForumException("No topics were found with such Id", "topicId");

            await _unitOfWork.TopicRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public IEnumerable<TopicModel> FindTopicsByTitleKeyWord(string titleKeyWord)
        {
            var topics = _unitOfWork.TopicRepository.FindAllWithDetails();
            var searchingTopics = topics.Where(t => t.Title.Contains(titleKeyWord)).ToList();
            return _mapper.Map<IEnumerable<TopicModel>>(searchingTopics);

        }

        public IEnumerable<TopicModel> FindTopicsByUserNickName(string userNikName)
        {
            var topics = _unitOfWork.TopicRepository.FindAllWithDetails();
            var user = _unitOfWork.ApplicationUserRepository.GetAllWithDetails()
                                                              .FirstOrDefault(u => u.NickName.ToUpper() == userNikName.ToUpper());

            if (user == null)
                throw new ForumException("No user was found with such NickName", "User NickName");

            var searchingTopics = topics.Where(t => t.Messages.Any(m=>m.ApplicationUserId==user.Id)).ToList();
            return _mapper.Map<IEnumerable<TopicModel>>(searchingTopics);
        }

        public IEnumerable<TopicModel> GetAll()
        {
            var topics = _unitOfWork.TopicRepository.FindAllWithDetails().ToList();

            return _mapper.Map<IEnumerable<TopicModel>>(topics);
        }

        public async Task<TopicModel> GetByIdAsync(int id)
        {
            var topic = await _unitOfWork.TopicRepository.GetByIdAsync(id);

            if (topic == null)
                throw new ForumException("No topic was found with such Id", "Topic Id");

            return _mapper.Map<TopicModel>(topic);
        }

        public IEnumerable<PopularityTopicModel> GetMostPopularTopicsNUmber(int topicCount)
        {
            var messages = _unitOfWork.MessageRepository.GetAllWithDetails();
            var topics = _unitOfWork.TopicRepository.FindAllWithDetails();

            var mostPopularTopics = messages.GroupBy(t => t.TopicId)
                                            .Select(m => new PopularityTopicModel
                                            {
                                                TopicTitle = topics.FirstOrDefault(t=>t.Id==m.Key).Title,
                                                UserCount = topics.First(t=>t.Id==m.Key).Messages.Select(u=>u.ApplicationUserId).Distinct().Count()
                                            })
                                            .OrderByDescending(c=>c.UserCount)
                                            .Take(topicCount);
            return mostPopularTopics;
        }

        public async Task UpdateAsync(TopicModel model, ClaimsPrincipal claims)
        {
            if (model == null)
                throw new ForumException("Topic can't be null", "TopicModel");

            var user = _userManager.GetUserAsync(claims).Result;

            if (!_userManager.GetRolesAsync(user).Result.Any(r => r == "admin"))
                throw new ForumException("Only admin can update topic", "claims");

            var topic = await _unitOfWork.TopicRepository.GetByIdAsync(model.Id);

            if (topic == null)
                throw new ForumException("No topic was found", "Topic Id");

            if (String.IsNullOrEmpty(model.Title))
                throw new ForumException("Topic can't be without title", "TopicModel");

            var updatetTopic = _mapper.Map<Topic>(model);
            
            _unitOfWork.TopicRepository.Update(updatetTopic);

            await _unitOfWork.SaveAsync();
        }
    }
}
