using GymSystemApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymSystemApp.DAL.Repositories.Contracts
{
    public interface IBookingRepo:IGenericRepo<Booking>
    {
        public Task<IEnumerable<Booking>> GetBookingsBySessionIdAsync(int sessionId, CancellationToken ct = default);
    }
}
