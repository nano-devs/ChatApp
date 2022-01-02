namespace ChatApp.Api.Services.Repositories;

using System.Linq.Expressions;

using ChatApp.Api.Data;

using Microsoft.EntityFrameworkCore;

public class Repository<T> : IRepository<T> where T : class
{
	protected readonly ChatAppDbContext _context;

	public Repository(ChatAppDbContext context)
	{
		this._context = context;
	}

	public T? GetById<TKey>(TKey id) where TKey : struct
	{
		return this._context.Set<T>().Find(id);
	}

	public async Task<T?> GetByIdAsync<TKey>(TKey? id) where TKey : struct
	{
		if (id == null)
		{
			throw new NullReferenceException();
		}

		return await this._context.Set<T>().FindAsync(id);
	}

	public IEnumerable<T> GetAll()
	{
		return this._context.Set<T>()
			.AsNoTrackingWithIdentityResolution();
	}

	public async Task<IEnumerable<T>> GetAllAsync()
	{
		return await this._context.Set<T>()
			.AsNoTrackingWithIdentityResolution()
			.ToListAsync();
	}

	public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
	{
		return this._context.Set<T>().Where(expression)
			.AsNoTrackingWithIdentityResolution();
	}

	public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
	{
		return await this._context.Set<T>().Where(expression)
			.AsNoTrackingWithIdentityResolution()
			.ToListAsync();
	}

	public async Task<IEnumerable<T>> GetPagedReponseAsync(int pageNumber, int pageSize)
	{
		return await this._context
			.Set<T>()
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.AsNoTrackingWithIdentityResolution()
			.ToListAsync();
	}

	public void Add(T entity)
	{
		this._context.Set<T>().Add(entity);
	}

	public void AddRange(IEnumerable<T> entities)
	{
		this._context.Set<T>().AddRange(entities);
	}

	public async Task AddAsync(T entity)
	{
		await this._context.Set<T>().AddAsync(entity);
	}

	public async Task AddRangeAsync(IEnumerable<T> entities)
	{
		await this._context.Set<T>().AddRangeAsync(entities);
	}

	public void Update(T entity)
	{
		this._context.Set<T>().Update(entity);
	}

	public void UpdateRange(IEnumerable<T> entities)
	{
		this._context.Set<T>().UpdateRange(entities);
	}

	public Task UpdateAsync(T entity)
	{
		try
		{
			this._context.Set<T>().Update(entity);
		}
		catch
		{
			this._context.Entry(entity).CurrentValues.SetValues(entity);
		}

		return Task.CompletedTask;
	}

	public Task UpdateRangeAsync(IEnumerable<T> entities)
	{
		try
		{
			this._context.Set<T>().UpdateRange(entities);
		}
		catch
		{
			this._context.Entry(entities).CurrentValues.SetValues(entities);
		}

		return Task.CompletedTask;
	}

	public void Remove(T entity)
	{
		this._context.Set<T>().Remove(entity);
	}

	public void RemoveRange(IEnumerable<T> entities)
	{
		this._context.Set<T>().RemoveRange(entities);
	}

	public Task RemoveAsync(T entity)
	{
		this._context.Set<T>().Remove(entity);
		return Task.CompletedTask;
	}

	public Task RemoveRangeAsync(IEnumerable<T> entities)
	{
		this._context.Set<T>().RemoveRange(entities);
		return Task.CompletedTask;
	}

	public int Save()
	{
		return this._context.SaveChanges();
	}

	public async Task<int> SaveAsync()
	{
		return await this._context.SaveChangesAsync();
	}
}
