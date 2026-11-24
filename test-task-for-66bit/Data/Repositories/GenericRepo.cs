using Application.Interfaces.RepoIntrefaces;
using Data.DB;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Repositories
{
    // Собирает повторяющиеся операции над DbSet. Запросы для конкретных сущностей определены в их репозиториях
    public class GenericRepo<TEntity> : IGenericRepo<TEntity> where TEntity : class
    {
        protected readonly DbSet<TEntity> _dbContext;

        public GenericRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext.Set<TEntity>();
        }

        public Task<List<TEntity>> ListAsync(CancellationToken ct)
            => _dbContext.AsNoTracking().ToListAsync(ct);

        public Task<TEntity?> FindAsync(Guid id, CancellationToken ct)
            => _dbContext.FindAsync(new object[] { id }, ct).AsTask();

        public void Add(TEntity entity) => _dbContext.Add(entity);

        public void Remove(TEntity entity) => _dbContext.Remove(entity);

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct)
            => await _dbContext.AnyAsync(predicate, ct);
    }
}
