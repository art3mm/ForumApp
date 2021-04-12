using DAL.Interfaces;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ForumDbContext _dbContext;
        private IMessageRepository _messageRepository;
        private IApplicationUserRepository _applicationUserRepository;
        private ITopicRepository _topicRepository;

        public UnitOfWork(ForumDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IApplicationUserRepository ApplicationUserRepository
        {
            get
            {
                if (_applicationUserRepository == null)
                    _applicationUserRepository = new ApplicationUserRepository(_dbContext);
                return _applicationUserRepository;
            }
        }
        
        public ITopicRepository TopicRepository
        {
            get
            {
                if (_topicRepository == null)
                    _topicRepository = new TopicRepository(_dbContext);
                return _topicRepository;
            }
        }

        public IMessageRepository MessageRepository
        {
            get
            {
                if (_messageRepository == null)
                    _messageRepository = new MessageRepository(_dbContext);
                return _messageRepository;
            }
        }

        public async Task<int> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
