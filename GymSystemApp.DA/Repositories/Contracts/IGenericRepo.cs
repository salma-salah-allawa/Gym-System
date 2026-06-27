using GymSystemApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymSystemApp.DAL.Repositories.Contracts
{
    public interface IGenericRepo<TEntitiy> where TEntitiy : BaseEntity, new()
    {
        Task<IEnumerable<TEntitiy>> GetAll(bool isTracked, CancellationToken cancellation = default);

        Task<TEntitiy?> GetById(int id, CancellationToken cancellation = default);

        void Add(TEntitiy entitiy);

        void Update(TEntitiy entitiy);

        void Delete(int id);
        public void Delete(TEntitiy entity);

        Task<TEntitiy?> FirstOrDefaultAsync(Expression<Func<TEntitiy, bool>> predicate,bool isTracked=false, CancellationToken cancellation = default);

        Task<bool> AnyAsync(Expression<Func<TEntitiy, bool>> predicate, CancellationToken cancellation = default);

        Task<int> CountAsync(Expression<Func<TEntitiy, bool>> predicate = null, CancellationToken cancellation = default);
    }
}
