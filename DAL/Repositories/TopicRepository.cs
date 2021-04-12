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
    public class TopicRepository : ITopicRepository
    {
        private readonly ForumDbContext _dbContext;

        public TopicRepository(ForumDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Topic entity)
        {
            await _dbContext.Topics.AddAsync(entity);
        }

        public void Delete(Topic entity)
        {
            _dbContext.Topics.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var topic = await _dbContext.Topics.FindAsync(id);

            _dbContext.Topics.Remove(topic);
        }

        public IQueryable<Topic> FindAll()
        {
            return _dbContext.Topics.Select(t => t);
        }

        public IQueryable<Topic> FindAllWithDetails()
        {
            return _dbContext.Topics.Include(m => m.Messages).Select(t => t);
        }

        public async Task<Topic> GetByIdAsync(int id)
        {
            return await _dbContext.Topics.FindAsync(id);
        }

        public async Task<Topic> GetByIdWithDetailsAsync(int id)
        {
            return await _dbContext.Topics.Include(m => m.Messages).FirstOrDefaultAsync(t => t.Id == id);
        }

        public void Update(Topic entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
