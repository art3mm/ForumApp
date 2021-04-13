using BLL.Interfaces;
using BLL.Models;
using BLL.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly ITopicService _topicService;
        private readonly IMessageService _messageService;

        public TopicsController(ITopicService topicService, IMessageService messageService)
        {
            _topicService = topicService;
            _messageService = messageService;
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> AddNewTopicAsync([FromBody] TopicModel topicModel)
        {
            try
            {
                topicModel.Id = await _topicService.AddAsync(topicModel, this.User);
                return Created(new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}/{topicModel.Id}", UriKind.Absolute), topicModel);
            }
            catch (ForumException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{topicId}")]
        public async Task<ActionResult> DeleteTopicByIdAsync(int topicId)
        {
            try
            {
                await _topicService.DeleteByIdAsync(topicId, this.User);
                return Ok($"Topic with Id={topicId} is deleted");
            }
            catch (ForumException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<TopicModel>> FindTopicsByTitleKeyWord([FromQuery] string titleKeyWord)
        {
            if (titleKeyWord == null)
                return GetAllTopics();

            var topics = _topicService.FindTopicsByTitleKeyWord(titleKeyWord);

            if (topics.Count() == 0)
                return Ok("No topics were found");

            return Ok(topics);
        }

        [AllowAnonymous]
        [HttpGet("user/{userNickName}")]
        public ActionResult<IEnumerable<TopicModel>> FindTopicsByUserNickName(string userNickName)
        {
            if (userNickName == null)
                return GetAllTopics();

            var topics = _topicService.FindTopicsByUserNickName(userNickName);

            if (topics.Count() == 0)
                return Ok($"No topics were found for nick {userNickName}");

            return Ok(topics);
        }


        public ActionResult<IEnumerable<TopicModel>> GetAllTopics()
        {
            var topics = _topicService.GetAll();

            if (topics.Count() == 0)
                return Ok("No topics were found");

            return Ok(topics);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{topicId}")]
        public async Task<ActionResult<TopicModel>> GetTopicByIdAsync(int topicId)
        {
            try
            {
                var topic = await _topicService.GetByIdAsync(topicId);
                return Ok(topic);
            }
            catch (ForumException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<ActionResult> UpdateTopicAsync([FromBody]TopicModel topicModel)
        {
            try
            {
                await _topicService.UpdateAsync(topicModel, this.User);
                return Ok(topicModel);
            }
            catch (ForumException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("{topicId}/messages")]
        public async Task<ActionResult> AddMessageToTopic(int topicId, [FromBody]MessageModel messageModel)
        {
            try
            {
                messageModel.TopicId = topicId;
                messageModel.Id = await _messageService.AddAsync(messageModel, this.User);
                return Created(new Uri($"{Request.Scheme}://{Request.Host}/api/messages/{messageModel.Id}", UriKind.Absolute), messageModel);
            }
            catch (ForumException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("popularTopics")]
        public ActionResult<IEnumerable<PopularityTopicModel>> GetMostPopularTopicsNUmber([FromQuery]int topicCount)
        {
            var popularTopics = _topicService.GetMostPopularTopicsNUmber(topicCount);

            if (popularTopics.Count() == 0)
                return Ok("No topics were found");


            return Ok(popularTopics);
        }
    }

    
}
