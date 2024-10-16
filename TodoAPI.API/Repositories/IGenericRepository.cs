﻿namespace TodoAPI.API.Repositories;

public interface IGenericRepository<T, Tid>
{
	// Get
	public Task<T?> GetByID(Tid id);
	public IQueryable<T> GetAll();

	// Create
	public Task<T?> Create(T entity);

	// Delete
	public Task<bool> SoftDelete(Tid id);
	public Task<bool> HardDelete(Tid id);

	// Update
	public Task<T?> Update(T entity);
}