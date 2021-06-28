using System.Linq;
using System.Threading.Tasks;
using BackEndAPI.Interfaces;
using BackEndAPI.DBContext;
using Microsoft.EntityFrameworkCore;

namespace BackEndAPI.Repositories
{
    public abstract class AsyncGenericRepository<TEntity> : IAsyncRepository<TEntity>
        where TEntity : class
    {

        private protected readonly AssetsManagementDBContext _context;

        public AsyncGenericRepository(AssetsManagementDBContext context)
        {
            _context = context;
        }

        public async Task<TEntity> Create(TEntity entity)
        {
             _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }

        public async Task<TEntity> GetById(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(TEntity entity)
        {            
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }

    }
}