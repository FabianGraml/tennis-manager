using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
namespace Tennis.Repository.GenericRepository;
public interface IGenericRepository<T> where T : class
{
    Task<T?> GetIncludingAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes);
    Task<IReadOnlyList<T>?> GetAllIncludingAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
    Task<T?> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void Remove(T entity);
    void Update(T entity);
}