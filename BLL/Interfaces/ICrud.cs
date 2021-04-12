using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ICrud<TModel> where TModel : class
    {
        IEnumerable<TModel> GetAll();

        Task<TModel> GetByIdAsync(int id);

        Task<int> AddAsync(TModel model, ClaimsPrincipal claims);

        Task UpdateAsync(TModel model, ClaimsPrincipal claims);

        Task DeleteByIdAsync(int modelId, ClaimsPrincipal claims);
    }
}
