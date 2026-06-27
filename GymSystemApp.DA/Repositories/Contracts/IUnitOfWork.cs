using GymSystemApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemApp.DAL.Repositories.Contracts
{
    public interface IUnitOfWork
    {
        public IGenericRepo<TEntity> GetRepo<TEntity>() where TEntity : BaseEntity, new();
        public Task<int> SaveChangesAsync();

        public ISessionRepo SessionRepo { get; }
        public IMembershipRepo MembershipRepo { get; }
        public IBookingRepo BookingRepo { get; }
    }
}
