using System.Linq;
using System.Threading.Tasks;

namespace BackEndAPI.Interfaces
{
    public interface IAsyncRepository<TEntity>
    {
        IQueryable<TEntity> GetAll();

        Task<TEntity> GetById(int id);

        Task<TEntity> Create(TEntity entity);

        Task Update(TEntity entity);

        Task Delete(TEntity entity);
    }
}