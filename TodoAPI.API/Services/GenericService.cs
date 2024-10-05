
using TodoAPI.API.Extensions;
using TodoAPI.API.Repositories;

namespace TodoAPI.API.Services;

public class GenericService<T, Tid> : IGenericService<T, Tid>
{
	readonly IGenericRepository<T, Tid> _repository;

	public GenericService(IGenericRepository<T, Tid> repository)
	{
		_repository = repository;
	}

	public async Task<T?> Create(T task) => await _repository.Create(task);

	public IQueryable<T> GetAll(int limit = 0)
		=> _repository.GetAll().TakeLimit(limit);

	public Task<T?> GetByID(Tid id) => _repository.GetByID(id);

	public virtual Task<bool> HardDelete(Tid id) => _repository.HardDelete(id);

	public virtual Task<bool> SoftDelete(Tid id) => _repository.SoftDelete(id);

	public virtual Task<T?> Update(T task) => _repository.Update(task);
}
