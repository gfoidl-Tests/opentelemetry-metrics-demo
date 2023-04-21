using System.Linq.Expressions;
using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using BookStore.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    protected readonly BookStoreDbContext Db;

    protected readonly DbSet<TEntity> DbSet;

    protected Repository(BookStoreDbContext db)
    {
        Db = db;
        DbSet = db.Set<TEntity>();
    }

    public virtual async Task Add(TEntity entity)
    {
        DbSet.Add(entity);
        await this.SaveChanges();
    }

    public virtual async Task<List<TEntity>> GetAll() => await DbSet.ToListAsync();

    public virtual async Task<TEntity?> GetById(int id) => await DbSet.FindAsync(id);

    public virtual async Task Update(TEntity entity)
    {
        DbSet.Update(entity);
        await this.SaveChanges();
    }

    public virtual async Task UpdateRange(IEnumerable<TEntity> entities)
    {
        DbSet.UpdateRange(entities);
        await this.SaveChanges();
    }

    public virtual async Task Remove(TEntity entity)
    {
        DbSet.Remove(entity);
        await this.SaveChanges();
    }

    public async Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate) => await DbSet.AsNoTracking().Where(predicate).ToListAsync();

    public async Task<int> SaveChanges() => await Db.SaveChangesAsync();

    public void Dispose() => Db?.Dispose();
}
