using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Interfaces
{
    public interface IMessageRepository : IRepository<Message>
    {
        IQueryable<Message> GetAllWithDetails();
    }
}
