using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IApplicationUserRepository
    {
        IQueryable<ApplicationUser> GetAllWithDetails();
        Task<ApplicationUser> GetByNickNameWithDetails(string userNickName);
        void UpdateUserStaus(ApplicationUser applicationUser);
        Task DeleteUserByIdAsync(string Id);
    }
}
