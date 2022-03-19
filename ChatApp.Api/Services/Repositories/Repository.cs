namespace ChatApp.Api.Services.Repositories;

using ChatApp.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class Repository<T, TKey> : IRepository<T, TKey> where T : class
{
	protected readonly ChatAppDbContext _context;

	public Repository(ChatAppDbContext context)
	{
		if (context is null)
		{
			throw new NullReferenceException("context");
		}

		_context = context;
	}

	public ChatAppDbContext Context
	{
		protected set { }
		get 
		{
			return this._context;
		}
	}

	public virtual T? GetById(TKey id)
	{
		return _context.Set<T>().Find(id);
	}

	public virtual async Task<T?> GetByIdAsync(TKey id)
	{
		return await _context.Set<T>().FindAsync(id);
	}

	public IEnumerable<T> GetAll()
	{
		return _context.Set<T>()
			.AsNoTrackingWithIdentityResolution();
	}

	public async Task<IEnumerable<T>> GetAllAsync()
	{
		return await _context.Set<T>()
			.AsNoTrackingWithIdentityResolution()
			.ToListAsync();
	}

	public virtual IEnumerable<T> Find(Expression<Func<T, bool>> expression)
	{
		return _context.Set<T>().Where(expression)
			.AsNoTrackingWithIdentityResolution();
	}

	public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
	{
		return await _context.Set<T>().Where(expression)
			.AsNoTrackingWithIdentityResolution()
			.ToListAsync();
	}

	public async Task<IEnumerable<T>> GetPagedReponseAsync(int pageNumber, int pageSize)
	{
		return await _context
			.Set<T>()
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.AsNoTrackingWithIdentityResolution()
			.ToListAsync();
	}

	public void Add(T entity)
	{
		_context.Set<T>().Add(entity);
	}

	public void AddRange(IEnumerable<T> entities)
	{
		_context.Set<T>().AddRange(entities);
	}

	public async Task AddAsync(T entity)
	{
		await _context.Set<T>().AddAsync(entity);
	}

	public async Task AddRangeAsync(IEnumerable<T> entities)
	{
		await _context.Set<T>().AddRangeAsync(entities);
	}

	public void Update(T entity)
	{
		_context.Set<T>().Update(entity);
	}

	public void UpdateRange(IEnumerable<T> entities)
	{
		_context.Set<T>().UpdateRange(entities);
	}

	public Task UpdateAsync(T entity)
	{
		try
		{
			_context.Set<T>().Update(entity);
		}
		catch
		{
			_context.Entry(entity).CurrentValues.SetValues(entity);
		}

		return Task.CompletedTask;
	}

	public Task UpdateRangeAsync(IEnumerable<T> entities)
	{
		try
		{
			_context.Set<T>().UpdateRange(entities);
		}
		catch
		{
			_context.Entry(entities).CurrentValues.SetValues(entities);
		}

		return Task.CompletedTask;
	}

	public void Remove(T entity)
	{
		_context.Set<T>().Remove(entity);
	}

	public void RemoveRange(IEnumerable<T> entities)
	{
		_context.Set<T>().RemoveRange(entities);
	}

	public Task RemoveAsync(T entity)
	{
		_context.Set<T>().Remove(entity);
		return Task.CompletedTask;
	}

	public Task RemoveRangeAsync(IEnumerable<T> entities)
	{
		_context.Set<T>().RemoveRange(entities);
		return Task.CompletedTask;
	}

	public int Save()
	{
		return _context.SaveChanges();
	}

	public async Task<int> SaveAsync()
	{
		return await _context.SaveChangesAsync();
	}
}
