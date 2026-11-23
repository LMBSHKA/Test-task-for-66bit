using System.Linq.Expressions;

namespace Application.Interfaces.RepoIntrefaces
{
    public interface IGenericRepo<TEntity> where TEntity : class
    {
        Task<List<TEntity>> ListAsync(CancellationToken ct);
        Task<TEntity?> FindAsync(Guid id, CancellationToken ct);
        void Add(TEntity entity);
        void Remove(TEntity entity);
        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);
    }
}
