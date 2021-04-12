using BLL.Models;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace BLL.Interfaces
{
    public interface IMessageService : ICrud<MessageModel>
    {
        IEnumerable<MessageModel> SearchMessagesByFilter(FilterSearchModel searchModel);
        IEnumerable<CurrentUserMessageModel> GetCurrentUsersMessages(ClaimsPrincipal claims);
    }
}
