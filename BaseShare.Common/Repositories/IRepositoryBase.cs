using System.Linq.Expressions;

namespace BaseShare.Common.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string? includeString = null,
            bool disableTracking = true);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            List<Expression<Func<T, object>>>? includes = null,
            bool disableTracking = true);


        Task<T?> GetByIdAsync(int id);
        Task<T?> GetByGuidAsync(Guid guid);
        Task<T> CreateAsync(T entity);
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
