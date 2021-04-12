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
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        
        [Authorize]
        [HttpDelete("{messageId}")]
        public async Task<ActionResult> DeleteMessageByIdAsync(int messageId)
        {
            try
            {
                await _messageService.DeleteByIdAsync(messageId, this.User);
                return Ok($"Message with Id={messageId} is deleted");
            }
            catch (ForumException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<MessageModel>> SearchMessagesByFilter([FromQuery] FilterSearchModel searchModel)
        {
            var messages = _messageService.SearchMessagesByFilter(searchModel);

            if (messages.Count() == 0)
                return Ok("No messages were found");

            return Ok(messages);
        }

        [Authorize]
        [HttpGet("{messageId}")]
        public async Task<ActionResult<MessageModel>> GetMessageByIdAsync(int messageId)
        {
            try
            {
                var message = await _messageService.GetByIdAsync(messageId);
                return Ok(message);
            }
            catch (ForumException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("{messageId}")]
        public async Task<ActionResult> UpdateMessageAsync(MessageModel messageModel, int messageId)
        {
            try
            {
                messageModel.Id = messageId;
                await _messageService.UpdateAsync(messageModel, this.User);
                return Ok(messageModel);
            }
            catch (ForumException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
