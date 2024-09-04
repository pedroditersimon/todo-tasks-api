namespace TodoAPI.Repositories;

public interface IGenericRepository<T, Tid>
{
    // Get
    public Task<T?> GetByID(Tid id);
    public IQueryable<T> GetAll(int limit = 50);

    // Create
    public Task<T?> Create(T task);

    // Delete
    public Task<bool> SoftDelete(Tid id);
    public Task<bool> HardDelete(Tid id);

    // Update
    public Task<T?> Update(T task);
}