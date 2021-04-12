using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ForumDbContext _dbContext;

        public MessageRepository(ForumDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Message entity)
        {
            await _dbContext.Messages.AddAsync(entity);
        }

        public void Delete(Message entity)
        {
            _dbContext.Messages.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var message = await _dbContext.Messages.FindAsync(id);

            _dbContext.Messages.Remove(message);
        }

        public IQueryable<Message> FindAll()
        {
            return _dbContext.Messages.Select(m=>m);
        }

        public IQueryable<Message> GetAllWithDetails()
        {
            return _dbContext.Messages
                             .Include(u => u.ApplicationUser)
                             .Include(t => t.Topic)
                             .Select(m => m);
        }

        public async Task<Message> GetByIdAsync(int id)
        {
            return await _dbContext.Messages.FindAsync(id);
        }

        public void Update(Message entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
