using GymSystemApp.Contexts;
using GymSystemApp.DAL.Entities;
using GymSystemApp.DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymSystemApp.DAL.Repositories.Classes
{
    public class GenericRepo<TEntitiy> : IGenericRepo<TEntitiy> where TEntitiy : BaseEntity, new()
    {
        private readonly GymAppDbContext dbContext;
        private readonly DbSet<TEntitiy> dbSet;
        public GenericRepo(GymAppDbContext _dbContext)
        {
            dbContext = _dbContext;
            dbSet= dbContext.Set<TEntitiy>();
        }
        public void Add(TEntitiy entitiy)
        {
            dbSet.Add(entitiy);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntitiy, bool>> predicate, CancellationToken cancellation = default)
        {
            return await dbSet.AnyAsync(predicate, cancellation);
        }

        public Task<int> CountAsync(Expression<Func<TEntitiy, bool>> predicate = null, CancellationToken cancellation = default)
        {
           return predicate is null ? dbSet.AsNoTracking().CountAsync(cancellation) : dbSet.AsNoTracking().CountAsync(predicate,cancellation);
        }

        public void Delete(int id)
        {
           var entity = dbSet.FirstOrDefault(e => e.Id == id);
            if (entity != null)
                dbSet.Remove(entity);
        }

        public void Delete(TEntitiy entity)
        {
            dbSet.Remove(entity);
        }

        public async Task<TEntitiy?> FirstOrDefaultAsync(Expression<Func<TEntitiy, bool>> predicate, bool isTracked = false, CancellationToken cancellation = default)
        {
            var query = isTracked ? dbSet : dbSet.AsNoTracking();
            return await query.FirstOrDefaultAsync(predicate, cancellation);
        }

        public async Task<IEnumerable<TEntitiy>> GetAll(bool isTracked, CancellationToken cancellation = default)
        {
            var entities = isTracked ? dbSet : dbSet.AsNoTracking();
            return await entities.ToListAsync();
        }

        public async Task<TEntitiy?> GetById(int id, CancellationToken cancellation = default)
        {
            return await dbSet.FirstOrDefaultAsync(e => e.Id == id);
        }

        public void Update(TEntitiy entitiy)
        {
            dbSet.Update(entitiy);
        }
    }
}
