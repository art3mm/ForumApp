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
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly ForumDbContext _dbContext;

        public ApplicationUserRepository(ForumDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DeleteUserByIdAsync(string Id)
        {
            var user = await _dbContext.Users.FindAsync(Id);
            _dbContext.Entry(user).Reference(c => c.UserProfile).Load();
            _dbContext.Users.Remove(user);
        }

        public IQueryable<ApplicationUser> GetAllWithDetails()
        {
            return _dbContext.Users.Include(p => p.UserProfile).Select(u => u);
        }

        public async Task<ApplicationUser> GetByNickNameWithDetails(string userNickName)
        {
            return await _dbContext.Users.Include(p => p.UserProfile)
                                         .SingleOrDefaultAsync(u => u.NickName == userNickName);
        }

        public void UpdateUserStaus(ApplicationUser applicationUser)
        {
            _dbContext.Entry(applicationUser).State = EntityState.Modified;
        }
    }
}
