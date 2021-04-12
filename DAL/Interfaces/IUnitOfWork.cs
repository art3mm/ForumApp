using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IApplicationUserRepository ApplicationUserRepository { get; }
        ITopicRepository TopicRepository { get; }
        IMessageRepository MessageRepository { get; }
        Task<int> SaveAsync();
    }
}
