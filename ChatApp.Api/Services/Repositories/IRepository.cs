namespace ChatApp.Api.Services.Repositories;

using System.Linq.Expressions;

public interface IRepository<T, TKey> where T : class
{
	T? GetById(TKey id);

	Task<T?> GetByIdAsync(TKey id);

	IEnumerable<T> GetAll();

	Task<IEnumerable<T>> GetAllAsync();

	IEnumerable<T> Find(Expression<Func<T, bool>> expression);

	Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);

	Task<IEnumerable<T>> GetPagedReponseAsync(int pageNumber, int pageSize);

	void Add(T entity);

	void AddRange(IEnumerable<T> entities);

	Task AddAsync(T entity);

	Task AddRangeAsync(IEnumerable<T> entities);

	void Update(T entity);

	void UpdateRange(IEnumerable<T> entities);

	Task UpdateAsync(T entity);

	Task UpdateRangeAsync(IEnumerable<T> entities);

	void Remove(T entity);

	void RemoveRange(IEnumerable<T> entities);

	Task RemoveAsync(T entity);

	Task RemoveRangeAsync(IEnumerable<T> entities);

	int Save();

	Task<int> SaveAsync();
}
