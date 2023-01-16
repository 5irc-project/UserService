using Microsoft.AspNetCore.Mvc;
using UserService.Models.EntityFramework;

namespace UserService.Models.Repository
{
    public interface IDataRepository<TEntity>
    {
        Task<ActionResult<IEnumerable<TEntity>>> GetAllAsync();
        Task<ActionResult<TEntity>> GetByIdAsync(int id);
        Task<ActionResult<TEntity>> GetByStringAsync(string str);
        Task AddAsync(TEntity entity);
        Task<ActionResult<User>> UpdateAsync(TEntity entityToUpdate, TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}
