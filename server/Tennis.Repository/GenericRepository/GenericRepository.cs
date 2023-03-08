using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Tennis.Database.Context;
namespace Tennis.Repository.GenericRepository;
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly TennisContext _dbContext;
    private readonly DbSet<T> _entitySet;
    public GenericRepository(TennisContext dbContext)
    {
        _dbContext = dbContext;
        _entitySet = _dbContext.Set<T>();
    }
    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        => await _dbContext.AddAsync(entity, cancellationToken);
    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _entitySet.ToListAsync(cancellationToken);
    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        => await _entitySet.Where(expression).ToListAsync(cancellationToken);
    public async Task<T?> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
         => await _entitySet.FirstOrDefaultAsync(expression, cancellationToken);
    public void Remove(T entity)
        => _dbContext.Remove(entity);
    public void Update(T entity)
        => _dbContext.Update(entity);
    public async Task<IReadOnlyList<T>?> GetAllIncludingAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes)
    {
        IQueryable<T> query = _dbContext.Set<T>();
        if (includes != null)
        {
            query = includes(query);
        }
        if (expression != null)
        {
            query = query.Where(expression);
        }
        return query != null ? await query.ToListAsync() : null;
    }
    public async Task<T?> GetIncludingAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes)
    {
        IQueryable<T> query = _dbContext.Set<T>();
        if (includes != null)
        {
            query = includes(query);
        }
        if (expression != null)
        {
            query = query.Where(expression);
        }
        return await query.FirstOrDefaultAsync();
    }
}