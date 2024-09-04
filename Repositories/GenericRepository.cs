﻿
using Microsoft.EntityFrameworkCore;
using TodoAPI.Models;

namespace TodoAPI.Repositories;

public class GenericRepository<T, Tid>(DbContext dbContext) : IGenericRepository<T, Tid>
    where T : EntityBaseModel<Tid>
    where Tid : IEquatable<Tid>
{
    protected DbSet<T> Entities => dbContext.Set<T>();


    #region Get
    public virtual async Task<T?> GetByID(Tid id)
        => await Entities.SingleOrDefaultAsync(t => t.ID.Equals(id));

    public virtual IQueryable<T> GetAll(int limit)
        => Entities;
    #endregion


    #region Create
    public virtual async Task<T?> Create(T entity)
           => Entities.Add(entity).Entity;
    #endregion


    #region Delete
    public virtual async Task<bool> HardDelete(Tid id)
    {
        T? entity = await GetByID(id);
        if (entity == null)
            return false;

        Entities.Remove(entity);
        return true;
    }

    public virtual async Task<bool> SoftDelete(Tid id)
    {
        T? entity = await GetByID(id);
        if (entity == null)
            return false;

        entity.IsDeleted = true;
        await Update(entity);

        return true;
    }
    #endregion


    #region Update
    public virtual async Task<T?> Update(T entity)
    {
        T? existingEntity = await GetByID(entity.ID);
        if (existingEntity == null)
            return null;

        dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
        return existingEntity;
    }
    #endregion
}
